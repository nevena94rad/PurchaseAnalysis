using Baza.Algorithm;
using Baza.GPISelector;
using Baza.Prepare;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Calculators
{
    public abstract class Abs_Calculator
    {
        protected static System.ComponentModel.BackgroundWorker worker = null;
        protected static event System.Action OnProgressUpdate;
        protected static event System.Action<string> OnProgressFinish;
        protected static ILog log = LogManager.GetLogger(typeof(Abs_Calculator));

        protected static int TotalCount = 0;
        protected static int DoneCount = 0;
        public static int totalWrites = 0;
        public static string message = "";
        public string displayText { get; set; }

        public List<I_PrepareDisplay> allAvailablePreparers = null;
        public static I_GPISelector gpiSelector = null;
        
        public Abs_Calculator (System.Action OnProgressUpdate, System.Action<string> OnProgressFinish, System.ComponentModel.BackgroundWorker worker)
        {
            Abs_Calculator.OnProgressUpdate = OnProgressUpdate;
            Abs_Calculator.OnProgressFinish = OnProgressFinish;
            Abs_Calculator.worker = worker;
        }
        
        //// upise u parent total count i done count i uradi on progressupdate 
        protected void UpdateProgress()
        {
            PredictionMaker.TotalCount = TotalCount;
            PredictionMaker.DoneCount = DoneCount;
            PredictionMaker.totalWrites = totalWrites;

            OnProgressUpdate?.Invoke();
        }
        /// sta god treba da uradi kad se zavrsi
        protected void Finish()
        {
            OnProgressFinish?.Invoke(message);
        }
        public abstract void makePrediction(int date);
        public abstract void setPreparer(I_PrepareDisplay preparer);

    }
}
