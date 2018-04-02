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

                PredictAllItems(data.AllCustomers[index]);
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

        }
    }
}
