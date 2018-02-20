using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public class Customer
    {
        public List<string> itemNos;
        public string custNo;
        public static int Tcount = 0;
        public static int Dcount = 0;
        public static int totalWrites = 0;
        public static Object thisLock = new Object();
        public static event System.Action OnProgressUpdate;

        public void getAllItems(int date)
        {
            // ucitati sve iteme koje je cust narucio vise od 2 puta (3+)

            
            DateTime bDate = Prediction.intToDateTime(date);
            //var allItems1 = from customer in db.PurchaseHistories
            //               where customer.CustNo == custNo && customer.InvDate < date
            //               group customer by customer.ItemNo into groupedCustomer
            //               where groupedCustomer.Count() > 2
            //               select new { ItemNo = groupedCustomer.Key, max = groupedCustomer.ToList().Max(x => x.InvDate) };
            
            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            string queryString = "select distinct(itemNo) from " + 
                ConfigurationManager.AppSettings[name: "PurchasePeriods"]+
                " where CustNo= @custNo and InvDateCurr<@bDate" +
                " group by itemNo " + "having count(*)>1 and max(InvDateCurr)>@bDateMinus6Months " +
                " and min(PurchasePeriod) * 0.5< DATEDIFF(DAY, max(InvDateCurr), @bDate) + 7" +
                " and max(PurchasePeriod) * 1.5 > DATEDIFF(DAY, max(InvDateCurr), @bDate)";

            itemNos = new List<string>();
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@custNo", custNo);
                command.Parameters.AddWithValue("@bDate", bDate.ToShortDateString());
                command.Parameters.AddWithValue("@bDateMinus6Months", bDate.AddMonths(-6).ToShortDateString());
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        itemNos.Add((String)reader[0]);
                    }
                    
                }
            }
            //var db = new DataClasses1DataContext();
            //var allItems = from period in db.PurchasePeriods
            //               where period.CustNo == custNo && period.InvDateCurr < bDate
            //               group period by period.ItemNo into groupedPeriod
            //               where groupedPeriod.Count() > 1
            //               select new
            //               {
            //                   ItemNo = groupedPeriod.Key,
            //                   last = groupedPeriod.ToList().Max(x => x.InvDateCurr),
            //                   min = groupedPeriod.ToList().Min(x => x.PurchasePeriod1),
            //                   max = groupedPeriod.ToList().Max(x => x.PurchasePeriod1)
            //               };

            //var items = allItems.ToList();
            //items.RemoveAll(x => x.last < bDate.AddMonths(-6) || x.min * 0.5 > (bDate - x.last).Days + 7 || x.max * 1.5 < (bDate - x.last).Days);

            //var itemNos1 = (from item in items
            //          select item.ItemNo).Distinct().ToList();
        }
        public List<Prediction> makeAllPredictions(int date)
        {
            // za svaki item se prave predikcije
            // za svaku predikciju se pamti za koj period je izvrsena, kad je predvidjena i kad se desila sl kupovina

            List<Prediction> returnList = new List<Prediction>();

            var db = new DataClasses1DataContext();
            
            foreach (var item in itemNos)
            {
                var allDates = (from purchase in db.PurchaseHistories
                               where purchase.CustNo == custNo && purchase.ItemNo == item
                               && purchase.InvDate < date
                               orderby purchase.InvDate
                               select purchase.InvDate).Distinct();

                List<int> listOfDates = allDates.ToList();

                var end = listOfDates.Last();
                var start = (from element in listOfDates
                             where (Prediction.intToDateTime(end) - Prediction.intToDateTime(element)).Days < 700
                             select element).Min();
                

                int quantity = getPurchaseQuantity(item, end);

                var prediction = Prediction.makePrediction(custNo, item, start, end, date, quantity);

                if (prediction.predictedConsumption >= 0)
                    returnList.Add(prediction);
                
            }
            
            return returnList;
        }
        private int getPurchaseQuantity(string item, int date)
        {
            var db = new DataClasses1DataContext();

            var quantityQuery = (from purchase in db.PurchaseHistories
                                 where purchase.CustNo == custNo &&
                                         purchase.ItemNo == item &&
                                         purchase.InvDate == date
                                 select purchase.InvQty).Sum();


            return quantityQuery;
        }
        public void PredictAllItems(int date)
        {
            getAllItems(date);
            List<Prediction> allCustomerPredictions = makeAllPredictions(date);
            allCustomerPredictions.OrderBy(x => x.to);

            var db = new DataClasses1DataContext();

            int predictionCount = allCustomerPredictions.Count();

            for (int i = 0; i < predictionCount; i++)
            {
                PurchasePrediction newPrediction = new PurchasePrediction();
                newPrediction.ItemNo = allCustomerPredictions[i].itemNo;
                newPrediction.CustNo = custNo;
                newPrediction.ProcessingDate = date;
                db.PurchasePredictions.InsertOnSubmit(newPrediction);
            }

            lock(thisLock)
            {
                totalWrites += predictionCount;
            }
            db.SubmitChanges();
        }
        
        public static void nextWeekPredictions(int date, Action t1_OnProgressUpdate)
        {
            ItemConsumption.readAllItemData(date);
            OnProgressUpdate += t1_OnProgressUpdate;
            List<Customer> returnList = new List<Customer>();

            var db = new DataClasses1DataContext();

            var allCustomers = (from customer in db.PurchaseHistories
                                select customer.CustNo).Distinct();

            int custCount = allCustomers.Count();
            var listCust = allCustomers.ToList();
            Tcount = custCount;
            Dcount = 0;
            Parallel.For(0, custCount,new ParallelOptions { MaxDegreeOfParallelism = 50}, i =>
            {
                Customer newCustomer = new Customer()
                {
                    custNo = listCust[i]
                };

                newCustomer.PredictAllItems(date);
                Dcount++;

                OnProgressUpdate?.Invoke();
            });
            
        }
    }
}
