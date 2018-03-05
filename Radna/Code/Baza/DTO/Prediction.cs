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
        public double predictedConsumption;
        public static REngine en = REngine.GetInstance();
        private static Object thisLock = new Object();


        public static void init()
        {
            en.Initialize();
            en.Evaluate("library(\"forecast\")");
            en.Evaluate("source("+ ConfigurationManager.AppSettings[name: "LoadingScript"] +")");
        }
        public static int doCustomer(string customerID, List<string> itemNos)
        { 

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
            string ItemID = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
            string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];
            string PurchaseQuantity = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseQuantity"];

            string queryString = "select " + ItemID + "," + PurchaseDate + ", " + PurchaseQuantity + " from " + Table +
                                   " where " + CustomerID + "= @CustID and " + PurchaseDate + "< @InvDate";

            List<string> ids = new List<string>();
            List<int> purchaseDates = new List<int>();
            List<int> quantities = new List<int>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@CustID", customerID);
                command.Parameters.AddWithValue("@InvDate", Parameters.processingDate);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (itemNos.Contains(reader[0].ToString()))
                        {
                            ids.Add(reader[0].ToString());
                            purchaseDates.Add((int)reader[1]);
                            quantities.Add((int)reader[2]);
                        }
                    }
                }
            }

            var file1 = TempFile.TempFileHelper.CreateTmpFile();
            using (var streamWriter = new StreamWriter(new FileStream(file1, FileMode.Open, FileAccess.Write)))
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    streamWriter.Write(itemNos.FindIndex(x => x == ids[i]) + "," + purchaseDates[i] + "," + quantities[i] + "\r\n");
                }
            }
            var file = file1.Replace('\\', '/');
            int modelID = ExecuteRScriptAlternativeWay(ConfigurationManager.AppSettings[name: "ExecuteScript"], file, Parameters.processingDate.ToString(), customerID, itemNos, Parameters.processingDate);
            TempFile.TempFileHelper.DeleteTmpFile(file1);

            return modelID;
        }
        public static double makePredictionBTYD(string cust, string item)
        {
            en.Evaluate("try(x <- cal.cbs[\""+item+"\", \"x\"])");
            en.Evaluate("try(t.x <- cal.cbs[\"" + item + "\", \"t.x\"])");
            en.Evaluate("try(T.cal <- cal.cbs[\"" + item + "\", \"T.cal\"])");
            return en.Evaluate("try(pnbd.ConditionalExpectedTransactions(params, T.star = 1, x, t.x, T.cal))").AsNumeric().First();
        }
        public static int ExecuteRScriptAlternativeWay(string rCodeFilePath, string p1, string p2, string cust, List<string> it, int date)
        {

            var args_r = new string[2] { p1, p2 };
            var execution = "source('" + rCodeFilePath + "')";

            lock (thisLock)
            {
                en.Evaluate("try(elog <- dc.ReadLines(\"" + p1 + "\", cust.idx = 1, date.idx = 2, sales.idx = 3))");
                en.Evaluate(execution);
                en.Evaluate("try(cal.cbs.dates <- data.frame(birth.periods, last.dates, as.Date(\"" + p2 + "\", \"%Y%m%d\")))");
                en.Evaluate("try(cal.cbs <- dc.BuildCBSFromCBTAndDates(cal.cbt, cal.cbs.dates, per = \"week\"))");

                var param = en.Evaluate("try(params <- pnbd.EstimateParameters(cal.cbs, max.param.value=100));").AsCharacter().ToString();

                var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
                string Table = ConfigurationManager.AppSettings[name: "CustomerModel"];
                string CustomerID = ConfigurationManager.AppSettings[name: "CustomerModel_CustomerID"];
                string Model = ConfigurationManager.AppSettings[name: "CustomerModel_Model"];
                string Parameters_ID = ConfigurationManager.AppSettings[name: "CustomerModel_Parameters_ID"];

                string queryString = "insert into " + Table + " (" + CustomerID + "," + Model + "," + Parameters_ID+"" +
                ") values (@CustNo, @Model, @Parameters_ID)";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    var command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@custNo", cust);
                    command.Parameters.AddWithValue("@Model", param);
                    command.Parameters.AddWithValue("@Parameters_ID", Parameters.databaseID);

                    return (int)command.ExecuteScalar();

                }
            }
        }
    }
}
