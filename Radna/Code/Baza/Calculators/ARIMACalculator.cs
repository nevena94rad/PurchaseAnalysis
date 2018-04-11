using Baza.Data;
using Baza.DTO;
using Baza.Prepare;
using Baza.R;
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
    public class ARIMACalculator : Calculator
    {
        protected ARIMAData data = null;
        public static REngine en = REngine.GetInstance();
        public static Object thisLock = new Object();
        public ARIMAPrepare preparer = null;

        public ARIMACalculator(System.Action OnProgressUpdate, System.Action<string> OnProgressFinish, System.ComponentModel.BackgroundWorker worker, ARIMAPrepare preparer) : base(OnProgressUpdate, OnProgressFinish, worker)
        {
            this.preparer = preparer;
        }

        public override void makePrediction(int date)
        {
            data = preparer.ARIMAprepare(date);

            int custCount = data.AllCustomers.Count;
            TotalCount = custCount;
            DoneCount = 0;
            bool stop = false;

            UpdateProgress();
            Parallel.For(0, custCount, new ParallelOptions { MaxDegreeOfParallelism = 1 }, (index, state) =>
            {

                if (worker.CancellationPending)
                {
                    stop = true;
                    state.Stop();
                    message = "The process has been canceled!";
                }
                if (!stop)
                {
                    predictAllItems(data.AllCustomers[index]);
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

            string queryString = "insert into ARIMA " + "( CustNo, ItemNo, consumption)" +
                " values (@CustNo, @ItemNo, @ProcessingValue)";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                for (int i = 0; i < predictionCount; i++)
                {
                    var command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@CustNo", customer);
                    command.Parameters.AddWithValue("@ItemNo", sortedPredictions[i].itemNo);
                    command.Parameters.AddWithValue("@ProcessingValue", sortedPredictions[i].predictedConsumption);

                    command.ExecuteNonQuery();
                }
            }
        }
        private List<Prediction> makeAllPredictions(ARIMACustomerData customer)
        {
            List<Prediction> returnList = new List<Prediction>();

            foreach(var item in customer.itemPurchased)
            {
                Prediction itemPrediction = makeItemPrediction(item, customer.Number);
                if (itemPrediction.predictedConsumption > 0)
                    returnList.Add(itemPrediction);
            }

            return returnList;
        }
        private Prediction makeItemPrediction(ARIMAItemData item, string customer)
        {
            Prediction retPredict = new Prediction();

            string dir = AppDomain.CurrentDomain.BaseDirectory;
            //string filePath = dir + @"R\ARIMA\RscriptFull.r";
            //string filePath = @"C:\Users\C3P_Dev13\Documents\GitHub\PurchaseAnalysis\RscriptFull.r";
            string filePath = @"C:\Users\C3P_Dev13\Documents\GitHub\PurchaseAnalysis\Radna\Code\WindowsFormsApp1\R\ARIMA\RscriptFull.r";
            int sum = (DateManipulation.intToDateTime(item.EndDate) - DateManipulation.intToDateTime(item.StartDate)).Days;

            string param1 = "";
            int j = 0;
            for (j = 0; j < item.customerConsumption.Count - 1; j++)
            {
                param1 = param1 + item.customerConsumption[j].Value + " ";
            }
            param1 = param1 + item.customerConsumption[j].Value;
            string param2 = item.globalConsumption.Count().ToString();
            DateTime start = DateManipulation.intToDateTime(item.StartDate);
            string param3 = start.Year.ToString();
            string param4 = start.DayOfYear.ToString();
            string param5 = sum.ToString();


            double predictConsumption = RConsoleHelper.ExecuteRScript(filePath, param1, param2, param3, param4, param5);
            predictConsumption += getScaledItemDate(item.globalConsumption.Select(x => x.Value).OrderBy(x => x).ToList(), item.customerConsumption.Average(x => x.Value), item.globalConsumption.Count() - item.customerConsumption.Count());
            

            retPredict.CustNo = customer;
            retPredict.itemNo = item.Number;
            int lastPurchase = Customer.getPurchaseQuantity(customer, item.Number, item.EndDate);
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
