using Baza.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baza.DTO;
using RDotNet;

namespace Baza.Calculators
{
    public class PNBDCalculator
    {
        protected PNBDData data = null;
        public static REngine en = REngine.GetInstance();
        public static Object thisLock = new Object();
        ///////ovde ide sve za pravljenje predikcije pomocu parenta

        /// <summary>
        /// pozove se prepareData i to se smesti u ovaj data, onda se sa tim radi
        /// </summary>
        public void makePrediction(int date)
        {
            initEngine();

            data = new PNBDData();
            data = Prepare.SimplePrepare.PNBDprepare(date);
            int custCount = data.AllCustomers.Count;

            Parallel.For(0, custCount, new ParallelOptions { MaxDegreeOfParallelism = 3 }, (index, state) =>
            {

                predictAllItems(data.AllCustomers[index]);
            });
        }
        public void initEngine()
        {
            en.Initialize();
            string dir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");
            string filePath = dir + "R/Functions.r";
            en.Evaluate("source('" + filePath + "')");
        }
        public void predictAllItems(string custNo)
        {
            data.AllItems = Customer.GetAllItems(custNo);
            data.LastPurchases = Customer.GetLastPurchases(custNo);

            List<Prediction> allCustomerPredictions = new List<Prediction>();
            lock (thisLock)
            {
                allCustomerPredictions = makeAllPredictions(custNo);
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

            Prediction.InsertPredictions(custNo, sortedPredictions, data.modelID);

        }
        public List<Prediction> makeAllPredictions(string custNo)
        {
            List<Prediction> returnList = new List<Prediction>();

            if (data.AllItems.Count() != 0)
                data.modelID = doCustomer(custNo, data.AllItems);

            if (data.modelID > 0)
            {
                for (var i = 0; i < data.AllItems.Count(); i++)
                {
                    var end = data.LastPurchases[i];

                    if (end != -1)
                    {
                        var predicted = Prediction.makePredictionBTYD(custNo, i.ToString());
                        var qty = Customer.getPurchaseQuantity(data.AllItems[i], end);

                        if (predicted >= qty && predicted < 100 * qty)
                        {
                            var percentage = 50 + 45 * Math.Pow((1 - qty / predicted), 10);
                            returnList.Add(new Prediction() { itemNo = data.AllItems[i], predictedConsumption = percentage });
                        }
                        else if (predicted > 100 * qty)
                        {
                            var percentage = 95;
                            returnList.Add(new Prediction() { itemNo = data.AllItems[i], predictedConsumption = percentage });
                        }
                        else if (predicted < qty)
                        {
                            var percentage = 50 * Math.Pow((predicted / qty), 10);
                            returnList.Add(new Prediction() { itemNo = data.AllItems[i], predictedConsumption = percentage });
                        }
                    }
                }
            }
            return returnList;
        }

        public int doCustomer(string custNo, List<string> items)
        {

        }
    }
}
