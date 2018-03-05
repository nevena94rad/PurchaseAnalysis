using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public static class Parameters
    {
        public static int customerRecency = 6;
        public static double predictionPercentageCutOff = 10;
        public static int predictionCountCutOff = 10;
        public static int processingDate;

        public static void LoadParameters(int date)
        {
            string cR = ConfigurationManager.AppSettings[name: "customerRecency"];
            if (cR != null)
                customerRecency = Int32.Parse(cR);

            string ppCO = ConfigurationManager.AppSettings[name: "predictionPercentageCutOff"];
            if (ppCO != null)
                predictionPercentageCutOff = Double.Parse(ppCO);

            string pCCO = ConfigurationManager.AppSettings[name: "predictionCountCutOff"];
            if (pCCO != null)
                predictionCountCutOff = Int32.Parse(pCCO);

            processingDate = date;
        }
    }
}
