using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDotNet;

namespace Baza.DTO
{
    public class Prediction
    {
        public string itemNo;
        public int from;
        public int to;
        public int occurred;
        public double predictedConsumption;
        public int lastInvQty;

        public static Prediction makePredictionAlternativeWay(string customer, string item, int begin, int end, int nextPurchase, int lastInvQty)
        {
            var db = new DataClasses1DataContext();

            var itemConsumption = (from purchases in db.ItemConsumptions
                                   where purchases.ItemNo == item && purchases.Date < nextPurchase
                                   && purchases.Date >= begin
                                   select new DailyValue { Value=purchases.Consumption, Date=purchases.Date }).OrderBy(x => x.Date);

            List<double> Consumption = new List<double>();
            foreach (var itemCons in itemConsumption)
            {
                Consumption.Add(itemCons.Value);
            }

            var customerConsumption = (from purchases in db.PurchaseHistories
                                      where purchases.CustNo == customer && purchases.ItemNo == item
                                      && purchases.InvDate < end && purchases.InvDate >= begin
                                      group purchases by purchases.InvDate into purchaseByDate
                                      select new DailyValue{ Date = purchaseByDate.Key, Value = purchaseByDate.Sum(x => x.InvQty) }).OrderBy(x=>x.Date);

            List<DailyValue> consumptionList = customerConsumption.ToList();
            consumptionList.RemoveAll(x => x.Value == 0);
            
            List<DailyValue> customerFullConsumption = TransformConsumption(consumptionList);

            List<double> Qty = new List<double>();
            foreach (var qty in customerFullConsumption)
            {
                Qty.Add(qty.Value);
            }

            int year1 = begin / 10000;
            int day1 = (intToDateTime(begin)).DayOfYear;
            int sum = (intToDateTime(end) - intToDateTime(begin)).Days;

            Prediction pred = new Prediction();
            pred.itemNo = item;
            pred.from = begin;
            pred.to = end;
            pred.occurred = nextPurchase;
            pred.lastInvQty = lastInvQty;

            string filePath = "C:/Users/rneve/Documents/GitHub/PurchaseAnalysis/R skripta/Rscript.r";

            string param1 = "";
            int i = 0;
            for (i = 0; i < Consumption.Count - 1; i++)
            {
                param1 = param1 + Consumption[i] + ", ";
            }
            param1 = param1 + Consumption[i];
            string param2 = "";
            int j = 0;
            for (j = 0; j < Qty.Count - 1; j++)
            {
                param2 = param2 + Qty[j] + ", ";
            }
            param2 = param2 + Qty[j];
            string param3 = year1.ToString();
            string param4 = day1.ToString();
            string param5 = sum.ToString();

            double predictConsumption = ExecuteRScript(filePath, param1, param2, param3, param4, param5);
            Prediction retPredict = new Prediction();
            retPredict.predictedConsumption = predictConsumption;

            return retPredict;
        }
        
        public static List<DailyValue> TransformConsumption(List<DailyValue> purchases)
        {
            List<DailyValue> returnList = new List<DailyValue>();
            DateTime current = intToDateTime(purchases[0].Date);
            DateTime? next = intToDateTime(purchases[1].Date);
            int daysBetween = ((DateTime)next - current).Days;
            int i = 0;
            int count = purchases.Count();

            while (next!=null )
            {
                returnList.Add(new DailyValue() { Value = purchases[i].Value / daysBetween, Date = DateTimeToint(current) });
                current = current.AddDays(1);
                if (current == next)
                {
                    ++i;
                    if (i + 1 < count)
                    {
                        next = intToDateTime(purchases[i + 1].Date);
                        daysBetween = ((DateTime)next - current).Days;
                    }
                    else
                        next = null;
                }
            }

            return returnList;
        }
        public static DateTime intToDateTime(int inDate)
        {
            int d = inDate % 100;
            int m = (inDate / 100) % 100;
            int y = inDate / 10000;

            return new DateTime(y, m, d);

        }
        public static int DateTimeToint(DateTime inDate)
        {
            return inDate.Year * 10000 + inDate.Month * 100 + inDate.Day;
        }
        
        public double getError()
        {
            return Math.Abs(1 - predictedConsumption / lastInvQty);
        }

        public static double ExecuteRScript(string rCodeFilePath, string p1, string p2, string p3, string p4, string p5)
        {
            using (var en = REngine.GetInstance())
            {
                var args_r = new string[5] { p1, p2, p3, p4, p5 };
                var execution = "source('" + rCodeFilePath + "')";
                en.Initialize();
                en.SetCommandLineArguments(args_r);
                var result = en.Evaluate(execution);
                //string ret = result.AsCharacter().First();
                double ret = result.AsNumeric().First();
                return ret;
            }
        }
    }
}
