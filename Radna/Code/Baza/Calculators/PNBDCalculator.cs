using Baza.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baza.DTO;
using RDotNet;
using System.IO;
using Baza.R;
using Baza.Prepare;
using System.Configuration;
using System.Data.SqlClient;

namespace Baza.Calculators
{
    public class PNBDCalculator : Abs_Calculator
    {
        protected PNBDData data = null;
        public static REngine en = REngine.GetInstance();
        public static Object thisLock = new Object();
        public I_PNBDPrepare preparer = null;

        public PNBDCalculator(System.Action OnProgressUpdate, System.Action<string> OnProgressFinish, System.ComponentModel.BackgroundWorker worker) :base(OnProgressUpdate, OnProgressFinish, worker)
        {
            displayText = "PNBD";
            allAvailablePreparers = PrepareCreator.CreatePNDBPrepare.getAll();
        }

        public override void setPreparer(I_PrepareDisplay preparer)
        {
            this.preparer = (I_PNBDPrepare) preparer;
        }

        public override void makePrediction(int date)
        {
            REngineHelper.initEngine();
            
            data = preparer.PNBDprepare(date);
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
                    data.AllCustomers[index].itemPurchased = new List<PNBDItemData>();
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
        
        public void predictAllItems(PNBDCustomerData customer)
        {

            List<Prediction> allCustomerPredictions = new List<Prediction>();
            lock (thisLock)
            {
                allCustomerPredictions = makeAllPredictions(customer);
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

            InsertPredictions(customer.Number, sortedPredictions, customer.modelID);

            lock (thisLock)
            {
                totalWrites += sortedPredictions.Count();

                DoneCount++;

                UpdateProgress();
            }

        }

        public List<Prediction> makeAllPredictions(PNBDCustomerData customer)
        {
            List<Prediction> returnList = new List<Prediction>();

            if (customer.NoPurchases != 0)
                doCustomer(customer);

            if (customer.modelID > 0)
            {
                for (var i = 0; i < customer.itemPurchased.Count(); i++)
                {
                    var end = customer.itemPurchased[i].LastPurchase;

                    if (end != -1)
                    {
                        var predicted = REngineHelper.PNBDgetItemPredictionData(customer.itemPurchased[i].Number);
                        var qty = Customer.getPurchaseQuantity(customer.Number, customer.itemPurchased[i].Number, end);

                        if (predicted >= qty && predicted < 100 * qty)
                        {
                            var percentage = 50 + 45 * Math.Pow((1 - qty / predicted), 10);
                            returnList.Add(new Prediction() { itemNo = customer.itemPurchased[i].Number, predictedConsumption = percentage });
                        }
                        else if (predicted > 100 * qty)
                        {
                            var percentage = 95;
                            returnList.Add(new Prediction() { itemNo = customer.itemPurchased[i].Number, predictedConsumption = percentage });
                        }
                        else if (predicted < qty)
                        {
                            var percentage = 50 * Math.Pow((predicted / qty), 10);
                            returnList.Add(new Prediction() { itemNo = customer.itemPurchased[i].Number, predictedConsumption = percentage });
                        }
                    }
                }
            }
            return returnList;
        }

        public void doCustomer(PNBDCustomerData customer)
        {
            var file1 = TempFile.TempFileHelper.CreateTmpFile();
            using (var streamWriter = new StreamWriter(new FileStream(file1, FileMode.Open, FileAccess.Write)))
            {
                foreach (var item in customer.itemPurchased)
                {
                    foreach (var purchase in item.purchases)
                        streamWriter.Write(item.Number + "," + purchase.purchaseDate + "," + purchase.purcaseQuantity + "\r\n");
                }
            }
            var file = file1.Replace('\\', '/');
            //string rCodeFilePath = ConfigurationManager.AppSettings[name: "ExecuteScript"];
            string dir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");
            string rCodeFilePath = dir + "R/PNBD/Script.r";
            int modelID = REngineHelper.PNBDExecuteRScript(rCodeFilePath, file, Parameters.processingDate.ToString(), customer.Number, Parameters.processingDate);
            TempFile.TempFileHelper.DeleteTmpFile(file1);

            customer.modelID = modelID;
        }

        public static void InsertPredictions(string custNo, List<Prediction> predictions, int modelID)
        {
            int predictionCount = predictions.Count();

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

                    if (Parameters.useGPI == false)
                        command.Parameters.AddWithValue("@ItemNo", predictions[i].itemNo);
                    else
                        //command.Parameters.AddWithValue("@ItemNo", gpiSelector.Select(custNo, predictions[i].itemNo));

                    command.Parameters.AddWithValue("@ProcessingValue", predictions[i].predictedConsumption);
                    command.Parameters.AddWithValue("@Model", modelID);

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
