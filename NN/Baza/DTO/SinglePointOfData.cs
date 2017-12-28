using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public enum category { two, five, ten, more }
    public class SinglePointOfData
    {
        public int numOfItemPurchases { get; set; }
        public double itemTwoPercent { get; set; }
        public double itemFivePercent { get; set; }
        public double itemTenPercent { get; set; }
        public double itemAbsouloutError { get; set; }

        public int numOfTotalPurchases { get; set; }
        public double totalTwoPercent { get; set; }
        public double totalFivePercent { get; set; }
        public double totalTenPercent { get; set; }
        public double totalAbsouloutError { get; set; }

        public category forecastCategory { get; set; }
        
        public double[] toArray()
        { 
            double[] returnArray = new double[10];

            returnArray[0] = numOfItemPurchases;
            returnArray[1] = itemTwoPercent;
            returnArray[2] = itemFivePercent;
            returnArray[3] = itemTenPercent;
            returnArray[4] = itemAbsouloutError;

            returnArray[5] = numOfTotalPurchases;
            returnArray[6] = totalTwoPercent;
            returnArray[7] = totalFivePercent;
            returnArray[8] = totalTenPercent;
            returnArray[9] = totalAbsouloutError;

            return returnArray;
        }
        public void load(DateTime start, DateTime end, String custNo, String itemNo)
        {
            //ucitavanje;
        }
    }
}
