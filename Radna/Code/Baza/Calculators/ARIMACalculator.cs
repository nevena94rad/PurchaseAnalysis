using Baza.Data;
using Baza.DTO;
using Baza.Prepare;
using RDotNet;
using System;
using System.Collections.Generic;
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
    }
}
