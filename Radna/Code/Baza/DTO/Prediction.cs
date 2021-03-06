﻿using System;
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
using log4net;
using RDotNet;

namespace Baza.DTO
{
    public class Prediction
    {
        public string CustNo = null;
        public string itemNo;
        public double predictedConsumption;

        public static REngine en = REngine.GetInstance();
        private static Object thisLock = new Object();
        private static ILog log = LogManager.GetLogger(typeof(Prediction));

        public static void init()
        {
            en.Initialize();
            //string filePath = ConfigurationManager.AppSettings[name: "LoadingScript"];
            string dir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");
            string filePath = dir + "R/Functions.r";
            en.Evaluate("source('" + filePath + "')");
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
                        if (itemNos.Contains(reader[0].ToString().Replace(" ", String.Empty)))
                        {
                            ids.Add(reader[0].ToString().Replace(" ", String.Empty));
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
            //string rCodeFilePath = ConfigurationManager.AppSettings[name: "ExecuteScript"];
            string dir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\","/");
            string rCodeFilePath = dir + "R/Script.r";
            int modelID = ExecuteRScriptAlternativeWay(rCodeFilePath, file, Parameters.processingDate.ToString(), customerID, itemNos, Parameters.processingDate);
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
                    string Table = ConfigurationManager.AppSettings[name: "CustomerModel"];
                    string CustomerID = ConfigurationManager.AppSettings[name: "CustomerModel_CustomerID"];
                    string Model = ConfigurationManager.AppSettings[name: "CustomerModel_Model"];
                    string Parameters_ID = ConfigurationManager.AppSettings[name: "CustomerModel_Parameters_ID"];

                    string queryString = "insert into " + Table + " (" + CustomerID + "," + Model + "," + Parameters_ID + "" +
                    ") OUTPUT INSERTED.ID values (@CustNo, @Model, @Parameters_ID) ";
                    queryString += @"SELECT SCOPE_IDENTITY();";

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        var command = new SqlCommand(queryString, connection);
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@custNo", cust);
                        command.Parameters.AddWithValue("@Model", model);
                        command.Parameters.AddWithValue("@Parameters_ID", Parameters.ID);

                        int idOfInserted = Convert.ToInt32(command.ExecuteScalar());
                        return idOfInserted;
                    }
                    
                }
                catch(Exception e)
                {
                    log.Warn(cust + " on: "+ date.ToString() + " " +e.Message);
                    return -1;
                }
            }
        }
    }
}
