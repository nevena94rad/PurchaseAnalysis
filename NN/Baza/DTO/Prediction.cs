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
        public static int succsess = 0;
        public static int fail = 0;

        public static Prediction makePrediction(string customer, string item, int begin, int end, int nextPurchase, int lastInvQty)
        {
            // na osnovu parametara potrebno je popuniti polja
            // posmatra se period begin do end i predvidja se kad ce sledeca kupovina da bude
           
            var db = new DataClasses1DataContext();

            Prediction pred = new Prediction();
            pred.itemNo = item;
            pred.from = begin;
            pred.to = end;
            pred.occurred = nextPurchase;
            pred.lastInvQty = lastInvQty;

            string connectionString = db.Connection.ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = new SqlCommand("Forecast", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.CommandTimeout = 300000;

            command.Parameters.Add(new SqlParameter("@param1", item));
            command.Parameters.Add(new SqlParameter("@param2", customer));
            command.Parameters.Add(new SqlParameter("@param3", begin));
            command.Parameters.Add(new SqlParameter("@param4", end));
            command.Parameters.Add(new SqlParameter("@param5", nextPurchase));
            

            DataTable dt = new DataTable();
            try
            {
                dt.Load(command.ExecuteReader());
                pred.predictedConsumption = Convert.ToDouble(dt.Rows[0]["Potrosnja"]);
                succsess++;

            }
            catch(Exception e)
            {
                fail++;
                pred.predictedConsumption = -1;
            }

            connection.Close();
            connection.Dispose();
           

            return pred;
        }

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

            List<double> Qty = new List<double>();
            foreach(var qty in customerConsumption)
            {
                Qty.Add(qty.Value);
            }

            List<DailyValue> consumptionList = customerConsumption.ToList();
            consumptionList.RemoveAll(x => x.Value == 0);
            consumptionList.Add(new DailyValue() { Date = end, Value=0 });
            List<DailyValue> customerFullConsumption = TransformConsumption(consumptionList);

            int year1 = begin / 10000;
            int day1 = (intToDateTime(begin)).DayOfYear;
            int sum = (intToDateTime(end) - intToDateTime(begin)).Days;

            string filePath = @"C:\Users\rneve\Documents\GitHub\PurchaseAnalysis\R skripta\proba.r";
            //string executablePath = @"C:\Program Files\R\R-3.4.3\bin\x64\Rscript.exe";

            string args = "\"";
            foreach(var cons in Consumption)
            {
                args = args + cons+ " ";
            }
            args = args + "\"" + "\"";
            foreach(var qty in Qty)
            {
                args = args + qty + " ";
            }
            args = args + "\"" + "\"";
            args = args + year1 + "\"" + "\"" + day1 + "\"" + "\"" + sum + "\"";

            int predictConsumption = ExecuteRScript(filePath, args);

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

        public static int ExecuteRScript(string rCodeFilePath, string args)
        {
            using (var en = REngine.GetInstance())
            {
                //var args_r = new string[2] { paramForScript1, paramForScript2 };
                var execution = "source('" + rCodeFilePath + "')";
                //en.SetCommandLineArguments(args_r);
                var result = en.Evaluate(execution);
                
                var ret = result.AsInteger().First();
                return ret;
            }
        }
    }
}
