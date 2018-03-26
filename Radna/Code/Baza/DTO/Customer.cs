using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Baza.DTO
{
    public class Customer
    {
        public List<string> itemNos;
        public List<int> lastPurchases;
        public string custNo;
        public int modelID = 0;

        public static int TotalCount = 0;
        public static int DoneCount = 0;
        public static int totalWrites = 0;
        public static Object thisLock = new Object();
        public static Object thisLock2 = new Object();
        public static event System.Action OnProgressUpdate;
        public static event System.Action<string> OnProgressFinish;
        public static System.ComponentModel.BackgroundWorker worker = null;
        private static ILog log = LogManager.GetLogger(typeof(Customer));

        public void getAllItems()
        {
            itemNos = new List<string>();
            lastPurchases = new List<int>();

            DateTime processingDateDateFormat = DateManipulation.intToDateTime(Parameters.processingDate);

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            string Table = ConfigurationManager.AppSettings[name: "PurchasePeriods"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchasePeriods_CustomerID"];
            string ItemID = ConfigurationManager.AppSettings[name: "PurchasePeriods_ItemID"];
            string Period = ConfigurationManager.AppSettings[name: "PurchasePeriods_Period"];
            string PeriodEnd = ConfigurationManager.AppSettings[name: "PurchasePeriods_PeriodEnd"];

            string queryString = "select distinct(" + ItemID + "),max(" + PeriodEnd + ") from " + Table +
                " where " + CustomerID + "= @custNo" + " and " + PeriodEnd + "<@bDate" +
                " group by " + ItemID + " having count(*)>1 and max(" + PeriodEnd + ")>@bDateMinus6Months " +
                " and min(" + Period + ") * 0.5< DATEDIFF(DAY, max(" + PeriodEnd + "), @bDate) + 7" +
                " and max(" + Period + ") * 1.5 > DATEDIFF(DAY, max(" + PeriodEnd + "), @bDate)";

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@custNo", custNo);
                command.Parameters.AddWithValue("@bDate", processingDateDateFormat.ToShortDateString());
                command.Parameters.AddWithValue("@bDateMinus6Months", processingDateDateFormat.AddMonths(-6).ToShortDateString());
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        itemNos.Add(((String)reader[0]).Replace(" ", String.Empty));
                        lastPurchases.Add(DateManipulation.DateTimeToint((DateTime)reader[1]));
                    }
                }
            }
        }

        public List<Prediction> makeAllPredictions()
        {
            List<Prediction> returnList = new List<Prediction>();

            if (itemNos.Count() != 0)
                modelID = Prediction.doCustomer(custNo, itemNos);

            if (modelID > 0)
            {
                for (var i = 0; i < itemNos.Count(); i++)
                {
                    var end = lastPurchases[i];

                    if (end != -1)
                    {
                        var predicted = Prediction.makePredictionBTYD(custNo, i.ToString());
                        var qty = getPurchaseQuantity(itemNos[i], end);

                        if (predicted >= qty && predicted < 100 * qty)
                        {
                            var percentage = 50 + 45 * Math.Pow((1 - qty / predicted), 10);
                            returnList.Add(new Prediction() { itemNo = itemNos[i], predictedConsumption = percentage });
                        }
                        else if (predicted > 100 * qty)
                        {
                            var percentage = 95;
                            returnList.Add(new Prediction() { itemNo = itemNos[i], predictedConsumption = percentage });
                        }
                        else if (predicted < qty)
                        {
                            var percentage = 50 * Math.Pow((predicted / qty), 10);
                            returnList.Add(new Prediction() { itemNo = itemNos[i], predictedConsumption = percentage });
                        }
                    }
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

        public void PredictAllItems()
        {
            getAllItems();
            List<Prediction> allCustomerPredictions = new List<Prediction>();
            lock (thisLock2)
            {
                allCustomerPredictions = makeAllPredictions();
            }

            allCustomerPredictions = allCustomerPredictions.OrderByDescending(x => x.predictedConsumption).ToList();
            List<Prediction> sortedPredictions = new List<Prediction>();
            int count = 0;
            foreach (Prediction pr in allCustomerPredictions)
            {
                if (pr.predictedConsumption >= Parameters.predictionPercentageCutOff)
                {
                    sortedPredictions.Add(pr);
                    count++;
                }
            }
            while (count <= Parameters.predictionCountCutOff && count < allCustomerPredictions.Count)
            {
                sortedPredictions.Add(allCustomerPredictions[count]);
                count++;
            }
            int predictionCount = sortedPredictions.Count();

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "PurchasePrediction"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchasePrediction_CustomerID"];
            string ItemID = ConfigurationManager.AppSettings[name: "PurchasePrediction_ItemID"];
            string ProcessingValue = ConfigurationManager.AppSettings[name: "PurchasePrediction_ProcessingValue"];
            string Model = ConfigurationManager.AppSettings[name: "PurchasePrediction_ModelID"];

            string queryString = "insert into " + Table + "(" + CustomerID + "," + ItemID + "," + ProcessingValue + "," + Model +
                ") values (@CustNo, @ItemNo, @ProcessingValue, @Model)";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                for (int i = 0; i < predictionCount; i++)
                {
                    var command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@CustNo", custNo);
                    command.Parameters.AddWithValue("@ItemNo", sortedPredictions[i].itemNo);
                    command.Parameters.AddWithValue("@ProcessingValue", sortedPredictions[i].predictedConsumption);
                    command.Parameters.AddWithValue("@Model", modelID);

                    command.ExecuteNonQuery();
                }
            }
            lock (thisLock)
            {
                totalWrites += predictionCount;

                DoneCount++;

                OnProgressUpdate?.Invoke();
            }
        }
        public static async Task nextWeekPredictionsAsync(int date, Action t1_OnProgressUpdate, Action<string> t2_OnFinishUpdate, System.ComponentModel.BackgroundWorker bWorker)
        {
            string message = "";
            bool stop = false;
            try
            {
                Prediction.init();
                OnProgressUpdate = t1_OnProgressUpdate;
                OnProgressFinish = t2_OnFinishUpdate;
                List<string> allCustomers = new List<string>();
                Customer.worker = bWorker;

                var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
                string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
                string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
                string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];

                string queryString = "select distinct(" + CustomerID + ") from " + Table + " where " + PurchaseDate + "< @date and " +
                    PurchaseDate + "> @dateMin";


                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@dateMin",
                        DateManipulation.DateTimeToint(DateManipulation.intToDateTime(date).AddMonths(-Parameters.customerRecency)));

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

                TotalCount = custCount;
                DoneCount = 0;
                totalWrites = 0;

                t1_OnProgressUpdate?.Invoke();

                Parallel.For(0, custCount, new ParallelOptions { MaxDegreeOfParallelism = 3 }, (index, state) =>
                {

                    Customer newCustomer = new Customer()
                    {
                        custNo = listCust[index]
                    };
                    if (worker.CancellationPending)
                    {
                        stop = true;
                        state.Stop();
                        message = "The process has been canceled!";
                    }
                    if (!stop)
                        newCustomer.PredictAllItems();
                });
            }
            catch(Exception ex)
            {
                message = ex.Message;
                log.Error(ex.Message);
            }

            if (DoneCount == TotalCount)
            {
                message = "Predictions have successfully been made";
                Parameters.Update((int)Enum.ProcessingStatus.Status.SUCCESS, "");
                OnProgressFinish?.Invoke(message);
            }
            else if(!stop)
            {
                Parameters.Update((int)Enum.ProcessingStatus.Status.ERROR, message);
                OnProgressFinish?.Invoke(message);
            }
            else
            {
                Parameters.Update((int)Enum.ProcessingStatus.Status.SUSPENDED, "");
                OnProgressFinish?.Invoke(message);
            }
        }
        public static DateTime GetLastTransactionDate()
        {
            int lastTransactionDate = 0;

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
            string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];

            string queryString = "select max(" + PurchaseDate + ") from " + Table;

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lastTransactionDate = (int)reader[0];
                    }
                }
            }

            return DateManipulation.intToDateTime(lastTransactionDate);
        }
    }
}

