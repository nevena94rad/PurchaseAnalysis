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
        public SinglePointOfData()
        {
            numOfItemPurchases = 0;
            itemTwoPercent = 0;
            itemFivePercent = 0;
            itemTenPercent = 0;
            itemAbsouloutError = 0;
            numOfTotalPurchases = 0;
            totalTwoPercent = 0;
            totalFivePercent = 0;
            totalTenPercent = 0;
            totalTenPercent = 0;
            totalAbsouloutError = 0;
        }
        public void addToItem(double error)
        {
            if (error < 0.02)
                itemTwoPercent++;
            else if (error < 0.05)
                itemFivePercent++;
            else if (error < 0.1)
                itemTenPercent++;
            else
                itemAbsouloutError++;

            numOfItemPurchases++;
        }
        public void addToTotall(double error)
        {
            if (error < 0.02)
                totalTwoPercent++;
            else if (error < 0.05)
                totalFivePercent++;
            else if (error < 0.1)
                totalTenPercent++;
            else
                totalTenPercent++;

            numOfTotalPurchases++;
        }
        public void addCategory(double error)
        {
            if (error < 0.02)
                forecastCategory = category.two;
            else if (error < 0.05)
                forecastCategory = category.five;
            else if (error < 0.1)
                forecastCategory = category.ten;
            else
                forecastCategory = category.more;
            
        }
        public void normalize()
        {
            itemTwoPercent /= numOfItemPurchases;
            itemFivePercent /= numOfItemPurchases;
            itemTwoPercent /= numOfItemPurchases;
            itemAbsouloutError /= numOfItemPurchases;

            totalTwoPercent /= numOfTotalPurchases;
            totalFivePercent /= numOfTotalPurchases;
            totalTenPercent /= numOfTotalPurchases;
            totalAbsouloutError /= numOfTotalPurchases;
        }
    }
}
