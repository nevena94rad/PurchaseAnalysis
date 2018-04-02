using Baza.Algorithm;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Calculators
{
    public abstract class Calculator
    {
        protected static System.ComponentModel.BackgroundWorker worker = null;
        protected static event System.Action OnProgressUpdate;
        protected static event System.Action<string> OnProgressFinish;
        protected static ILog log = LogManager.GetLogger(typeof(Calculator));

        protected static PredictionMaker parent = null;

        //// upise u parent total count i done count i uradi on progressupdate 
        protected void UpdateProgress()
        {
            
        }
        /// sta god treba da uradi kad se zavrsi
        protected void Finish()
        {
            
        }
        protected abstract void makePrediction();
        

    }
}
