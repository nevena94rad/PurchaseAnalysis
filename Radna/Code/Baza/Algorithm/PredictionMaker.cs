using Baza.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Algorithm
{
    public static class PredictionMaker
    {
        public static Abs_Calculator calculator { get; set; } = null;

        public static int TotalCount = 0;
        public static int DoneCount = 0;
        public static int totalWrites = 0;

        public static void startProccess(int date)
        {
            if (calculator != null)
                calculator.makePrediction(date);
        }
        
        public static List<Abs_Calculator> getAllCalculators(System.Action OnProgressUpdate, System.Action<string> OnProgressFinish, System.ComponentModel.BackgroundWorker worker)
        {
            List<Abs_Calculator> returnList = new List<Abs_Calculator>();

            returnList.Add(new ARIMACalculator(OnProgressUpdate, OnProgressFinish, worker));
            returnList.Add(new PNBDCalculator(OnProgressUpdate, OnProgressFinish, worker));

            return returnList;
        }
    }
}
