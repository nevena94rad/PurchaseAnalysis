using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
<<<<<<< HEAD
            command.CommandTimeout = 300000;
=======
            command.CommandTimeout = 3000000;
>>>>>>> b76adb3dded93a5f70b673a2866ec2c062905c46

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
                                   select new { purchases.Consumption, purchases.Date }).OrderBy(x => x.Date);

            var customerConsumption = (from purchases in db.PurchaseHistories
                                      where purchases.CustNo == customer && purchases.ItemNo == item
                                      && purchases.InvDate < nextPurchase && purchases.InvDate >= begin
                                      group purchases by purchases.InvDate into purchaseByDate
                                      select new { Date = purchaseByDate.Key, Qty = purchaseByDate.Sum(x => x.InvQty) }).OrderBy(x=>x.Qty);


                                      select new DailyValue{ Date = purchaseByDate.Key, Value = purchaseByDate.Sum(x => x.InvDate) }).OrderBy(x=>x.Qty);

            List<DailyValue> consumptionList = customerConsumption.ToList();
            consumptionList.RemoveAll(x => x.Value == 0);
            consumptionList.Add(new DailyValue() { Date = nextPurchase, Value=0 });
            List<DailyValue> customerFullConsumption = TransformConsumption(consumptionList);

            int year1 = begin / 1000;
            int day1 = (intToDateTime(begin)).DayOfYear;
            int sum = (intToDateTime(end) - intToDateTime(end)).Days;

             throw new Exception();
        }

        public static List<double> TransformConsumption(List<double> purchases)
        public static List<DailyValue> TransformConsumption(List<DailyValue> purchases)
        {
            List<double> returnList = new List<double>();

            List<DailyValue> returnList = new List<DailyValue>();
            DateTime current = intToDateTime(purchases[0].Date);
            DateTime next = intToDateTime(purchases[1].Date);
            int daysBetween = (next - current).Days;
            int i = 0;

            while (next!=null )
            {
                returnList.Add(new DailyValue() { Value = purchases[i].Value / daysBetween, Date = DateTimeToint(current) });
                current.AddDays(1);
                if (current == next)
                {
                    ++i;
                    next = intToDateTime(purchases[i + 1].Date);
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
        public double getError()
        {
            return Math.Abs(1 - predictedConsumption / lastInvQty);
        }

        public int ExecuteRScript(string rCodeFilePath, string rScriptExecutablePath, string args)
        {
            string file = rCodeFilePath;
            string result = string.Empty;

            try
            {

                var info = new ProcessStartInfo();
                info.FileName = rScriptExecutablePath;
                info.WorkingDirectory = Path.GetDirectoryName(rScriptExecutablePath);
                info.Arguments = rCodeFilePath + " " + args;

                info.RedirectStandardInput = false;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                info.CreateNoWindow = true;

                using (var proc = new Process())
                {
                    proc.StartInfo = info;
                    proc.Start();
                    result = proc.StandardOutput.ReadToEnd();
                }

                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception("R Script failed: " + result, ex);
            }
        }
    }
}
