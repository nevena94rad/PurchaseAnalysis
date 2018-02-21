using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
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
        public static int count=0;
        //public static REngine en = REngine.GetInstance();
        private static Object thisLock = new Object();
        private static BlockingCollection<LogRecord> LogQueue = new BlockingCollection<LogRecord>();


        public static void init()
        {
           // en.Initialize();
           // en.Evaluate("library(\"forecast\")");
        }
        public static Prediction makePrediction(string customer, string item, int begin, int end, int nextPurchase, int lastInvQty)
        {
            var now1 = DateTime.Now;

            Prediction retPredict = new Prediction();
            try
            {
                var db = new DataClasses1DataContext();

                //prvi deooooooooooooooooooooo
                //ItemConsumption.readAllItemData(nextPurchase);
                //var itemData = ConsumptionData.Instance.readData(item);
                //int daysToRemove = (intToDateTime(begin) - intToDateTime(itemData.startDate)).Days;
                //int dateCount = itemData.listOfValues.Count();

                //List<double> Consumption = new List<double>();
                //for (int i = daysToRemove; i < dateCount; i++)
                //    Consumption.Add(itemData.listOfValues[i]);

                var itemConsumption = (from purchases in db.ItemConsumptions
                                       where purchases.ItemNo == item && purchases.Date < nextPurchase && purchases.Date >= begin
                                       select new DailyValue { Value = purchases.Consumption, Date = purchases.Date }).OrderBy(x => x.Date);

                List<double> Consumption = new List<double>();
                foreach (var itemCons in itemConsumption)
                {
                    Consumption.Add(itemCons.Value);
                }
                /////////////////

                List<DailyValue> Consumptions = new List<DailyValue>();
                
                var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
                string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
                string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
                string ItemID = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
                string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];
                string PurchaseQuantity = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseQuantity"];

                string queryString = "select " + PurchaseDate + ", " + PurchaseQuantity + " from " + Table +
                                       " where "+ ItemID + "= @ItemID and " + CustomerID + "= @CustID and "+
                                       PurchaseDate + ">= @begin and " + PurchaseDate +"<= @end order by "+ PurchaseDate;

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@ItemID", item);
                    command.Parameters.AddWithValue("@CustID", customer);
                    command.Parameters.AddWithValue("@end", end);
                    command.Parameters.AddWithValue("@begin", begin);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Consumptions.Add(new DailyValue { Date = (int)reader[0], Value = (int)reader[1] });
                        }
                    }
                }

                List<DailyValue> consumptionList = (from purchases in Consumptions
                                                   group purchases by purchases.Date into purchaseByDate
                                                   select new DailyValue { Date = purchaseByDate.Key, Value = purchaseByDate.Sum(x => x.Value) }).OrderBy(x => x.Date).ToList();

                consumptionList.RemoveAll(x => x.Value == 0);

                if (consumptionList.Count() > 1)
                {
                    List<DailyValue> customerFullConsumption = TransformConsumption(consumptionList);

                    List<double> Qty = new List<double>();
                    foreach (var qty in customerFullConsumption)
                    {
                        Qty.Add(qty.Value);
                    }

                    int year1 = begin / 10000;
                    int day1 = (intToDateTime(begin)).DayOfYear;
                    int sum = (intToDateTime(end) - intToDateTime(begin)).Days;


                    string filePath = @"C:\Users\ne cackaj mi komp\Documents\GitHub\PurchaseAnalysis\R skripta\RscriptFull.r";
                    //string filePath2 = @"C:\Users\ne cackaj mi komp\Desktop\Finalna verzija\PurchaseAnalysis-master\R skripta\RscriptFull.r";

                    string param0 = "";
                    int i = 0;
                    for (i = 0; i < Consumption.Count - 1; i++)
                    {
                        param0 = param0 + Consumption[i] + " ";
                    }
                    param0 = param0 + Consumption[i];
                    string param1 = "";
                    int j = 0;
                    for (j = 0; j < Qty.Count - 1; j++)
                    {
                        param1 = param1 + Qty[j] + " ";
                    }
                    param1 = param1 + Qty[j];
                    string param2 = Consumption.Count().ToString();
                    string param3 = year1.ToString();
                    string param4 = day1.ToString();
                    string param5 = sum.ToString();


                    //LogRecord rec = new LogRecord() { p1 = param1, p2 = param2, p3 = param3, p4 = param4, p5 = param5, customer = customer, item = item, nextPurchase = nextPurchase, lastPurchase = lastInvQty };
                    double predictConsumption = ExecuteRScript(filePath, param1, param2, param3, param4, param5);
                    predictConsumption += getScaledItemDate(Consumption, customerFullConsumption.Average(x => x.Value), Consumption.Count() - Qty.Count()); 
                    //double predictConsumption2 = ExecuteRScriptAlternativWay(filePath2,param0, param2,param3, param4, param5);
                    //LogQueue.Add(rec);
                    //added++;
                    retPredict.from = begin;
                    retPredict.itemNo = item;
                    retPredict.lastInvQty = lastInvQty;
                    retPredict.occurred = nextPurchase;
                    retPredict.to = end;
                    retPredict.predictedConsumption = predictConsumption;

                    if (predictConsumption < lastInvQty)
                        retPredict.predictedConsumption = -1;
                    count++;

                    var diff = DateTime.Now - now1;
                    return retPredict;
                }
            }
            catch (Exception e)
            {

            }
            count++;

            retPredict.predictedConsumption = -1;
            return retPredict;
        }

        private static double getScaledItemDate(List<double> consumption, double customerAverage, int customerCount)
        {
            double returnValue = 0;
            double average = 0;
            int count = consumption.Count();

            for (int i = 0; i < count-customerCount; ++i)
                average += consumption[i];
            average = average / (count-customerCount);

            for (int i = (count - customerCount); i < count; ++i)
                returnValue += consumption[i]/average;

            return returnValue*customerAverage / 2;
        }

        //public static Prediction makePredictionAlternativeWay(string customer, string item, int begin, int end, int nextPurchase, int lastInvQty)
        //{
        //    Prediction retPredict = new Prediction();
        //    try
        //    {
        //        var db = new DataClasses1DataContext();

        //        var itemConsumption = (from purchases in db.ItemConsumptions
        //                               where purchases.ItemNo == item && purchases.Date < nextPurchase
        //                               && purchases.Date >= begin
        //                               select new DailyValue { Value = purchases.Consumption, Date = purchases.Date }).OrderBy(x => x.Date);



        //        List<double> consumption = new List<double>();
        //        foreach (var itemCons in itemConsumption)
        //        {
        //            consumption.Add(itemCons.Value);
        //        }

        //        var customerConsumption = (from purchases in db.PurchaseHistories
        //                                   where purchases.CustNo == customer && purchases.ItemNo == item
        //                                   && purchases.InvDate <= end && purchases.InvDate >= begin
        //                                   group purchases by purchases.InvDate into purchaseByDate
        //                                   select new DailyValue { Date = purchaseByDate.Key, Value = purchaseByDate.Sum(x => x.InvQty) }).OrderBy(x => x.Date);


        //        List<DailyValue> consumptionList = customerConsumption.ToList();
        //        consumptionList.RemoveAll(x => x.Value == 0);


        //        if (consumptionList.Count() > 1)
        //        {
        //            List<DailyValue> customerFullConsumption = TransformConsumption(consumptionList);


        //            List<double> Qty2 = new List<double>();
        //            foreach (var qty in customerFullConsumption)
        //            {
        //                Qty2.Add(qty.Value);
        //            }


        //            int year1 = begin / 10000;
        //            int day1 = (intToDateTime(begin)).DayOfYear;
        //            int sum = (intToDateTime(end) - intToDateTime(begin)).Days;


        //            double rezultat = -1;
        //            lock (thisLock)
        //            {
        //                NumericVector Consumption = en.CreateNumericVector(consumption);
        //                en.SetSymbol("Consumption", Consumption);
        //                NumericVector custItemConsumption = en.CreateNumericVector(Qty2);
        //                en.SetSymbol("custItemConsumption", custItemConsumption);

        //                en.Evaluate("custItemCons <- ts(custItemConsumption, start=c(" + year1 + ", " + day1 + "),frequency = 365.25)");
        //                en.Evaluate("vremenskiItem <-ts(Consumption,start=c(" + year1 + "," + day1 + "),frequency = 365.25)");
        //                var proba1 = en.Evaluate("itemMean <- head(Consumption," + sum + ")").AsNumeric();
        //                double fgh = en.Evaluate("itemMean <- mean(itemMean)").AsNumeric().First();
        //                NumericVector gdf = en.Evaluate("L1 <- (vremenskiItem/itemMean) * mean(custItemCons) ").AsNumeric();

        //                en.Evaluate("fit<-auto.arima(custItemCons, stepwise=FALSE, approximation=FALSE, lambda=0)");

        //                en.Evaluate("prognoza<-forecast(fit,h=(" + "length(Consumption) - " + sum + " + 7 " + "))");
        //                en.Evaluate("ItemCust<-c(custItemCons,head(prognoza[[\"mean\"]], -7))");
        //                var pr = en.Evaluate("rezultat<-(L1+ItemCust)/2").AsNumeric();

        //                rezultat = en.Evaluate("sum(tail(rezultat,-" + sum + "))+sum(tail(prognoza[[\"mean\"]],7))").AsNumeric().First();

        //                retPredict.from = begin;
        //                retPredict.itemNo = item;
        //                retPredict.lastInvQty = lastInvQty;
        //                retPredict.occurred = nextPurchase;
        //                retPredict.to = end;
        //                retPredict.predictedConsumption = rezultat;
        //            }
        //            count++;

        //            if (rezultat < lastInvQty)
        //                retPredict.predictedConsumption = -1;

        //            return retPredict;
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    count++;

        //    retPredict.predictedConsumption = -1;
        //    return retPredict;
        //}


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
        //public static double ExecuteRScriptAlternativeWay(LogRecord rec, string p1, string p2, string p3, string p4, string p5)
        //{

        //    var args_r = new string[5] { p1, p2, p3, p4, p5 };
        //    //var execution = "source('" + rCodeFilePath + "')";
        //    double ret;

        //    lock (thisLock)
        //    {
        //        poslednji = rec;
        //        stoper = 21;
        //        //stoper = 22;
        //        en.SetCommandLineArguments(args_r);

        //        var result = en.Evaluate("args <- commandArgs() \n" +
        //            "Consumption <- as.numeric(unlist(strsplit(args[2], \",\"))) \n" +
        //            "Qty <- as.numeric(unlist(strsplit(args[3], \",\"))) \n" +
        //            "year1 <- as.numeric(args[4]); \n" +
        //            "day1 <- as.numeric(args[5]); \n" +
        //            " sum <- as.numeric(args[6]); \n" +
        //            "# return (Qty[1]) \n" +
        //            "custItemCons <-ts(Qty, start = c(year1, day1), frequency = 365.25) \n" +
        //            "vremenskiItem <-ts(Consumption, start = c(year1, day1), frequency = 365.25) \n" +
        //            "itemMean <-head(Consumption, sum) \n" +
        //            "itemMean <-mean(itemMean) \n" +
        //            "L1 <-(vremenskiItem / itemMean) * mean(custItemCons) \n " +
        //            "fit <-auto.arima(custItemCons, stepwise = FALSE, approximation = FALSE, lambda = 0)         \n   " +
        //            "prognoza <-forecast(fit, h = (length(Consumption) - sum + 7))                \n                       " +
        //            "ItemCust <-c(custItemCons, head(prognoza[[\"mean\"]], -7)) \n" +
        //            "rezultat <-(L1 + ItemCust) / 2 \n" +
        //            "(sum(tail(rezultat, -sum)) + sum(tail(prognoza[[\"mean\"]], 7))) \n"
        //            );
        //        //stoper = 23;
        //        ret = result.AsNumeric().First();
        //        //stoper = 24;  
        //    }
        //    return ret;
        //}
        public static double ExecuteRScript(string rCodeFilePath, string p1, string p2, string p3, string p4, string p5)
        {

            string[] args_r = { p1, p2, p3, p4, p5 };
            double result = REngineRunner.RunFromCmd(rCodeFilePath, args_r);
            return result;
        }
    }
}
