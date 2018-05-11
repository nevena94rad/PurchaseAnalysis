using Baza.Data;
using Baza.DTO;
using Baza.Prepare;
using Baza.R;
using log4net;
using RDotNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Calculators
{
    public class ARIMACalculator : Abs_Calculator
    {
        protected ARIMAData data = null;
        public static REngine en = REngine.GetInstance();
        public static Object thisLock = new Object();
        public I_ARIMAPrepare preparer = null;
        private static ILog log = LogManager.GetLogger(typeof(ARIMACalculator));

        public ARIMACalculator(System.Action OnProgressUpdate, System.Action<string> OnProgressFinish, System.ComponentModel.BackgroundWorker worker) : base(OnProgressUpdate, OnProgressFinish, worker)
        {
            displayText = "ARIMA";
            allAvailablePreparers = PrepareCreator.CreateARIMAPrepare.getAll();
        }

        public override void setPreparer(I_PrepareDisplay preparer)
        {
            this.preparer = (I_ARIMAPrepare) preparer;
        }
        
        public override void makePrediction(int date)
        {
            data = preparer.ARIMAprepare(date);

            int custCount = data.AllCustomers.Count;
            TotalCount = custCount;
            DoneCount = 0;
            bool stop = false;

            UpdateProgress();
            Parallel.For(0, custCount, new ParallelOptions { MaxDegreeOfParallelism = 3 }, (index, state) =>
            {

                if (worker.CancellationPending)
                {
                    stop = true;
                    state.Stop();
                    message = "The process has been canceled!";
                }
                if (!stop)
                {
                    try
                    {
                        predictAllItems(data.AllCustomers[index]);
                    }
                    catch(Exception ex)
                    {
                        log.Error(ex.Message);
                    }
                    data.AllCustomers[index].itemPurchased = new List<ARIMAItemData>();
                }
            });

            if (DoneCount == TotalCount)
            {
                message = "Predictions have successfully been made";
                Parameters.Update((int)Enum.ProcessingStatus.Status.SUCCESS, "");
                Finish();
                log.Info("success");
            }
            else if (!stop)
            {
                Parameters.Update((int)Enum.ProcessingStatus.Status.ERROR, message);
                Finish();
                log.Error("fail");
            }
            else
            {
                Parameters.Update((int)Enum.ProcessingStatus.Status.SUSPENDED, "");
                Finish();
                log.Info("suspended");
            }
        }
        public void predictAllItems(ARIMACustomerData customer)
        {
            
            List<Prediction> allCustomerPredictions = makeAllPredictions(customer);


            int predictionCount = allCustomerPredictions.Count();

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

            InsertPredictions(customer.Number, sortedPredictions);

            lock (thisLock)
            {
                totalWrites += sortedPredictions.Count();

                DoneCount++;

                UpdateProgress();
            }

        }

        private void InsertPredictions(string customer, List<Prediction> sortedPredictions)
        {

            int predictionCount = sortedPredictions.Count();

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table1 = ConfigurationManager.AppSettings[name: "Model"];
            string Model_Model = ConfigurationManager.AppSettings[name: "Model_Model"];
            string Model_Parameters_ID = ConfigurationManager.AppSettings[name: "Model_Parameters_ID"];

            string Table2 = ConfigurationManager.AppSettings[name: "PurchasePrediction"];
            string PurchasePrediction_CustomerID = ConfigurationManager.AppSettings[name: "PurchasePrediction_CustomerID"];
            string PurchasePrediction_ItemID = ConfigurationManager.AppSettings[name: "PurchasePrediction_ItemID"];
            string PurchasePrediction_ProcessingValue = ConfigurationManager.AppSettings[name: "PurchasePrediction_ProcessingValue"];
            string PurchasePrediction_Model = ConfigurationManager.AppSettings[name: "PurchasePrediction_ModelID"];

            string queryString1 = "insert into " + Table1 + " (" + Model_Model + "," + Model_Parameters_ID + "" +
            ") OUTPUT INSERTED.ID values (@Model, @Parameters_ID) ";
            queryString1 += @"SELECT SCOPE_IDENTITY();";

            string queryString2 = "insert into " + Table2 + "(" + PurchasePrediction_CustomerID + "," + PurchasePrediction_ItemID + "," + PurchasePrediction_ProcessingValue + "," 
                + PurchasePrediction_Model + ") values (@CustNo, @ItemNo, @ProcessingValue, @Model)";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                for (int i = 0; i < predictionCount; i++)
                {
                    var command = new SqlCommand(queryString1, connection);
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Model", sortedPredictions[i].model);
                    command.Parameters.AddWithValue("@Parameters_ID", Parameters.ID);

                    int idOfInserted = Convert.ToInt32(command.ExecuteScalar());

                    var command2 = new SqlCommand(queryString2, connection);
                    command2.Parameters.AddWithValue("@CustNo", customer);
                    if (sortedPredictions[i].isGPI == false)
                        command2.Parameters.AddWithValue("@ItemNo", sortedPredictions[i].itemNo);
                    else
                        command2.Parameters.AddWithValue("@ItemNo", gpiSelector.Select(customer, sortedPredictions[i].itemNo));
                    command2.Parameters.AddWithValue("@ProcessingValue", sortedPredictions[i].predictedConsumption);
                    command2.Parameters.AddWithValue("@Model", idOfInserted);

                    command2.ExecuteNonQuery();
                }
            }
            
        }
        private List<Prediction> makeAllPredictions(ARIMACustomerData customer)
        {
            List<Prediction> returnList = new List<Prediction>();

            foreach (var item in customer.itemPurchased)
            {
                Prediction itemPrediction = makeItemPrediction(item, customer.Number);

                if (itemPrediction.predictedConsumption != -1)
                {
                    if (itemPrediction.predictedConsumption > 1 && itemPrediction.predictedConsumption < 3)
                    {
                        itemPrediction.predictedConsumption = 50 + 152 * Math.Pow((1 - 1.0 / itemPrediction.predictedConsumption), 3);
                    }
                    else if (itemPrediction.predictedConsumption >= 3)
                    {
                        itemPrediction.predictedConsumption = 855 * Math.Pow((1.0 / itemPrediction.predictedConsumption), 2);
                    }
                    else if (itemPrediction.predictedConsumption < 1)
                    {
                        itemPrediction.predictedConsumption = 50 * Math.Pow((itemPrediction.predictedConsumption), 2);
                    }

                    returnList.Add(itemPrediction);
                }
            }

            return returnList;
        }
        private Prediction makeItemPrediction(ARIMAItemData item, string customer)
        {
            Prediction retPredict = new Prediction();
            ARIMAItemData gc = new ARIMAItemData();
            lock (ARIMAData.thisLock)
            {
               gc = ARIMAData.AllItemData.Find(x => x.Number == item.Number);
            }
            ARIMAItemData globalConsumption = new ARIMAItemData() { Number = item.Number, StartDate = item.StartDate, EndDate = gc.EndDate, customerConsumption = new List<ARIMAConsumptionData>() };
            foreach(var el in gc.customerConsumption)
            {
                if (el.Date >= globalConsumption.StartDate)
                    globalConsumption.customerConsumption.Add(el);
            }

            string filePath = preparer.GetScriptPath();
            int sum = (DateManipulation.intToDateTime(item.EndDate) - DateManipulation.intToDateTime(item.StartDate)).Days;

            string param1 = "";
            int j = 0;
            for (j = 0; j < item.customerConsumption.Count - 1; j++)
            {
                param1 = param1 + item.customerConsumption[j].Value + " ";
            }
            param1 = param1 + item.customerConsumption[j].Value;
            string param2 = globalConsumption.customerConsumption.Count().ToString();
            DateTime start = DateManipulation.intToDateTime(item.StartDate);
            string param3 = start.Year.ToString();
            string param4 = start.DayOfYear.ToString();
            string param5 = sum.ToString();

            string model;
            double predictConsumption = RConsoleHelper.ExecuteRScript(filePath, param1, param2, param3, param4, param5, out model);
            predictConsumption += getScaledItemDate(globalConsumption.customerConsumption.Select(x => x.Value).OrderBy(x => x).ToList(), item.customerConsumption.Average(x => x.Value), globalConsumption.customerConsumption.Count() - item.customerConsumption.Count());
            

            retPredict.CustNo = customer;
            retPredict.itemNo = item.Number;
            retPredict.model = model;
            int lastPurchase = Customer.getPurchaseQuantity(customer, item.Number, item.EndDate, false);
            retPredict.predictedConsumption = lastPurchase > 0 ? (predictConsumption / lastPurchase) : -1;
            
           
            return retPredict;
        }

        private static double getScaledItemDate(List<double> consumption, double customerAverage, int customerCount)
        {
            double returnValue = 0;
            double average = 0;
            int count = consumption.Count();

            for (int i = 0; i < count - customerCount; ++i)
                average += consumption[i];
            average = average / (count - customerCount);

            for (int i = (count - customerCount); i < count; ++i)
                returnValue += consumption[i] / average;

            return returnValue * customerAverage / 2;
        }

    }
}
