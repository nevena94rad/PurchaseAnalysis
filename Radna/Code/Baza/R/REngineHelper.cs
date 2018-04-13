using Baza.DTO;
using log4net;
using RDotNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.R
{
    public static class REngineHelper
    {
        public static REngine en = REngine.GetInstance();
        private static Object thisLock = new Object();
        private static ILog log = LogManager.GetLogger(typeof(REngineHelper));

        public static void initEngine()
        {
            en.Initialize();
            string dir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");
            string filePath = dir + "R/PNBD/Functions.r";
            en.Evaluate("source('" + filePath + "')");
        }

        public static int PNBDExecuteRScript(string rCodeFilePath, string p1, string p2, string cust, int date)
        {

            var args_r = new string[2] { p1, p2 };
            var execution = "source('" + rCodeFilePath + "')";

            lock (thisLock)
            {
                try
                {
                    log.Info(cust);

                    en.Evaluate("try(elog <- dc.ReadLines(\"" + p1 + "\", cust.idx = 1, date.idx = 2, sales.idx = 3))");
                    en.Evaluate(execution);
                    en.Evaluate("try(cal.cbs.dates <- data.frame(birth.periods, last.dates, as.Date(\"" + p2 + "\", \"%Y%m%d\")))");
                    en.Evaluate("try(cal.cbs <- dc.BuildCBSFromCBTAndDates(cal.cbt, cal.cbs.dates, per = \"week\"))");

                    var param = en.Evaluate("try(withTimeout(params <- pnbd.EstimateParameters(cal.cbs, max.param.value=100), timeout=240, onTimeout=\"silent\"));").AsList();
                    var model = "r= " + param[0].AsNumeric().First() + " alpha= " + param[1].AsNumeric().First();
                    model += " s= " + param[2].AsNumeric().First() + " beta= " + param[3].AsNumeric().First();
                    var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
                    string Table = ConfigurationManager.AppSettings[name: "Model"];
                    string Model = ConfigurationManager.AppSettings[name: "Model_Model"];
                    string Parameters_ID = ConfigurationManager.AppSettings[name: "Model_Parameters_ID"];

                    string queryString = "insert into " + Table + " (" + Model + "," + Parameters_ID + "" +
                    ") OUTPUT INSERTED.ID values (@Model, @Parameters_ID) ";
                    queryString += @"SELECT SCOPE_IDENTITY();";

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        var command = new SqlCommand(queryString, connection);
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Model", model);
                        command.Parameters.AddWithValue("@Parameters_ID", Parameters.ID);

                        int idOfInserted = Convert.ToInt32(command.ExecuteScalar());
                        return idOfInserted;
                    }

                }
                catch (Exception e)
                {
                    log.Warn(cust + " on: " + date.ToString() + " " + e.Message);
                    return -1;
                }
            }
        }
        public static double PNBDgetItemPredictionData(string itemNo)
        {
            en.Evaluate("try(x <- cal.cbs[\"" + itemNo + "\", \"x\"])");
            en.Evaluate("try(t.x <- cal.cbs[\"" + itemNo + "\", \"t.x\"])");
            en.Evaluate("try(T.cal <- cal.cbs[\"" + itemNo + "\", \"T.cal\"])");
            return en.Evaluate("try(pnbd.ConditionalExpectedTransactions(params, T.star = 1, x, t.x, T.cal))").AsNumeric().First();
        }
        
    }
}
