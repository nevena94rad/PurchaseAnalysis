using System;
using System.Collections.Generic;
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

        public void getAllItems(int date)
        {
            // ucitati sve iteme koje je cust narucio vise od 2 puta (3+)
        
            var db = new DataClasses1DataContext();

            DateTime bDate = Prediction.intToDateTime(date);

            //var allItems1 = from customer in db.PurchaseHistories
            //               where customer.CustNo == custNo && customer.InvDate < date
            //               group customer by customer.ItemNo into groupedCustomer
            //               where groupedCustomer.Count() > 2
            //               select new { ItemNo = groupedCustomer.Key, max = groupedCustomer.ToList().Max(x => x.InvDate) };


            var allItems = from period in db.PurchasePeriods
                           where period.CustNo == custNo && period.InvDateCurr < bDate
                           group period by period.ItemNo into groupedPeriod
                           select new { ItemNo = groupedPeriod.Key, last = groupedPeriod.ToList().Max(x => x.InvDateCurr),
                                        min = groupedPeriod.ToList().Min(x => x.PurchasePeriod1),
                                        max = groupedPeriod.ToList().Max(x => x.PurchasePeriod1)};

            var items = allItems.ToList();
            items.RemoveAll(x => x.last < bDate.AddMonths(-6) || x.min * 0.5 > (bDate-x.last).Days+7 || x.max *1.5 < (bDate - x.last).Days );
           
            itemNos = (from item in items
                      select item.ItemNo).ToList();
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

                var start = listOfDates.First();
                var end = listOfDates.Last();

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

            for(int i = 0; i<predictionCount;i++)
            {
                PurchasePrediction newPrediction = new PurchasePrediction();
                newPrediction.ItemNo = allCustomerPredictions[i].itemNo;
                newPrediction.CustNo = custNo;
                newPrediction.ProcessingDate = date;
                db.PurchasePredictions.InsertOnSubmit(newPrediction);
            }

            db.SubmitChanges();
        }
        public static void nextWeekPredictions(int date)
        {
            List<Customer> returnList = new List<Customer>();

            var db = new DataClasses1DataContext();

            var allCustomers = (from customer in db.PurchaseHistories
                                select customer.CustNo).Distinct();

            int custCount = allCustomers.Count();
            var listCust = allCustomers.ToList();
            Parallel.For(0, custCount,new ParallelOptions { MaxDegreeOfParallelism = 50}, i =>
            {
                Customer newCustomer = new Customer()
                {
                    custNo = listCust[i]
                };

                newCustomer.PredictAllItems(date);
            });
            
        }
    }
}
