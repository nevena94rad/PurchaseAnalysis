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

        /// <summary>
        /// nevena
        /// </summary>
        /// <param name="date"></param>
        public void getAllItems(int date)
        {
            itemNos = new List<string>();
            DateTime bDate = Prediction.intToDateTime(date);
            
            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            string Table = ConfigurationManager.AppSettings[name: "PurchasePeriods"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchasePeriods_CustomerID"];
            string ItemID = ConfigurationManager.AppSettings[name: "PurchasePeriods_ItemID"];
            string Period = ConfigurationManager.AppSettings[name: "PurchasePeriods_Period"];
            string PeriodEnd = ConfigurationManager.AppSettings[name: "PurchasePeriods_PeriodEnd"];

            string queryString = "select distinct("+ItemID+") from " + Table +
                " where "+CustomerID+ "= @custNo" + " and " + PeriodEnd + "<@bDate" +
                " group by "+ItemID + " having count(*)>1 and max("+ PeriodEnd + ")>@bDateMinus6Months " +
                " and min(" + Period + ") * 0.5< DATEDIFF(DAY, max(" + PeriodEnd + "), @bDate) + 7" +
                " and max(" + Period + ") * 1.5 > DATEDIFF(DAY, max(" + PeriodEnd + "), @bDate)";

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
            
        }
        public List<Prediction> makeAllPredictions(int date)
        {
            
            List<Prediction> returnList = new List<Prediction>();
            
            
            foreach (var item in itemNos)
            {

                int start = -1;
                int end = -1;

                var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
                string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
                string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
                string ItemID = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
                string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];

                string queryString = "select min("+ PurchaseDate + "), max(" + PurchaseDate + ") from " +
                    Table + " where "+CustomerID+"=@custNo and "+ItemID+"=@itemNo and "+ PurchaseDate+"<@todaysDate";


                using (var connection = new SqlConnection(connectionString))
                {
                    var command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@custNo", custNo);
                    command.Parameters.AddWithValue("@itemNo", item);
                    command.Parameters.AddWithValue("@todaysDate", date);


                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            start = (int)reader[0];
                            end = (int)reader[1];
                        }

                    }
                }


                if (start != -1 && end != -1)
                {
                    int quantity = getPurchaseQuantity(item, end);

                    var prediction = Prediction.makePrediction(custNo, item, start, end, date, quantity);

                    if (prediction.predictedConsumption >= 0)
                        returnList.Add(prediction);
                }
            }
            
            return returnList;
        }
        private int getPurchaseQuantity(string item, int date)
        {
            int sum = 0;

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
            string ItemID = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
            string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];
            string PurchaseQuantity = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseQuantity"];

            string queryString = "select sum(" + PurchaseQuantity + ") from " + Table + 
                " where " + CustomerID + "=@custNo and " + ItemID + "=@itemNo and " + PurchaseDate + "=@definedDate";

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@custNo", custNo);
                command.Parameters.AddWithValue("@itemNo", item);
                command.Parameters.AddWithValue("@definedDate", date);


                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        sum = (int)reader[0];
                    }
                }
            }

            return sum;
        }
        public void PredictAllItems(int date)
        {
            getAllItems(date);
            List<Prediction> allCustomerPredictions = makeAllPredictions(date);


            int predictionCount = allCustomerPredictions.Count();

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "PurchasePrediction"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchasePrediction_CustomerID"];
            string ItemID = ConfigurationManager.AppSettings[name: "PurchasePrediction_ItemID"];
            string ProcessingDate = ConfigurationManager.AppSettings[name: "PurchasePrediction_ProcessingDate"];

            string queryString = "insert into "+ Table + "("+CustomerID+","+ItemID+","+ProcessingDate+") values (" +
                "@CustNo, @ItemNo, @ProcessingDate)";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                for (int i = 0; i < predictionCount; i++)
                {

                    var command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@custNo", custNo);
                    command.Parameters.AddWithValue("@itemNo", allCustomerPredictions[i].itemNo);
                    command.Parameters.AddWithValue("@ProcessingDate", date);

                    command.ExecuteNonQuery();
                
                }
            }
            lock(thisLock)
            {
                totalWrites += predictionCount;
            }
            
        }
        
        public static void nextWeekPredictions(int date, Action t1_OnProgressUpdate)
        {
            ItemConsumption.readAllItemData(date);
            OnProgressUpdate += t1_OnProgressUpdate;
            List<string> allCustomers = new List<string>();

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
            string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];

            string queryString = "select distinct(" + CustomerID + ") from " + Table+ " where " + PurchaseDate + "< @date";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@date", date);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allCustomers.Add(reader[0].ToString());
                    }
                }
            }

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
