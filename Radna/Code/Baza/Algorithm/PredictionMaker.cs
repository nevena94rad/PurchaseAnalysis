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
        public static AbsCalculator calculator { get; set; } = null;

        public static int TotalCount = 0;
        public static int DoneCount = 0;
        public static int totalWrites = 0;

        public static void startProccess(int date)
        {
            if (calculator != null)
                calculator.makePrediction(date);
        }
        
        public static List<AbsCalculator> getAllCalculators(System.Action OnProgressUpdate, System.Action<string> OnProgressFinish, System.ComponentModel.BackgroundWorker worker)
        {
            List<AbsCalculator> returnList = new List<AbsCalculator>();

            returnList.Add(new ARIMACalculator(OnProgressUpdate, OnProgressFinish, worker));
            returnList.Add(new PNBDCalculator(OnProgressUpdate, OnProgressFinish, worker));

            return returnList;
        }
    }
}
