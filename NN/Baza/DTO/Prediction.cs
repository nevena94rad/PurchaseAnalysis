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
