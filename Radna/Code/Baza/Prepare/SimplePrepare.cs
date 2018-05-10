using Baza.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baza.DTO;
using System.Configuration;
using System.Data.SqlClient;
using static Baza.DTO.TempFile;
using System.IO;

namespace Baza.Prepare
{
    public class SimplePrepare : I_PNBDPrepare ,I_ARIMAPrepare
    {
        public static string ScriptPath = null;

        public string displayName { get { return "Simple Prepare"; } }

        /// <summary>
        /// Ucitava i obradjuje podatke
        /// </summary>
        /// <returns></returns>
        public PNBDData PNBDprepare(int date)
        {
            PNBDData returnData = new PNBDData();
            PNBDData.date = date;
            PNBDCustomerData.preparer = this;

            List<string> customerIDs = PNBDreadAllCustomers();

            foreach (var customer in customerIDs)
                returnData.AllCustomers.Add(new PNBDCustomerData() { Number = customer });

            return returnData;
        }
        public List<string> PNBDreadAllCustomers()
        {
            List<string> allCustomers = new List<string>();

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
            string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];

            string queryString = "select distinct(" + CustomerID + ") from " + Table + " where " + PurchaseDate + "< @date and " +
                PurchaseDate + "> @dateMin";


            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@date", PNBDData.date);
                command.Parameters.AddWithValue("@dateMin",
                    DateManipulation.DateTimeToint(DateManipulation.intToDateTime(PNBDData.date).AddMonths(-Parameters.customerRecency)));

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allCustomers.Add(reader[0].ToString());
                    }
                }
            }

            return allCustomers;
        }

        public List<PNBDItemData> PNBDreadAllItems(string custNo)
        {
            List<PNBDItemData> returnList = new List<PNBDItemData>();
            List<string> distinctItems;
            string queryString;
            List<string> distinctItems2 = null;
            string queryString2 = null;
            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            if (Parameters.useGPI == false)
            {
                distinctItems = Customer.GetAllItems(custNo);
                
                string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
                string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
                string ItemID = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
                string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];
                string PurchaseQuantity = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseQuantity"];

                queryString = "select " + ItemID + "," + PurchaseDate + ", " + PurchaseQuantity + " from " + Table +
                                       " where " + CustomerID + "= @CustID and " + PurchaseDate + "< @InvDate";
            }
            else
            {
                distinctItems = Customer.GetAllItemsWithGPIs(custNo);

                string PurchaseHistory_Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
                string PurchaseHistory_CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
                string PurchaseHistory_ItemID = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
                string PurchaseHistory_PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];
                string PurchaseHistory_PurchaseQuantity = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseQuantity"];

                string ItemGPI_Table = ConfigurationManager.AppSettings[name: "ItemGPI"];
                string ItemGPI_GPI = ConfigurationManager.AppSettings[name: "ItemGPI_GPI"];
                string ItemGPI_ItemID = ConfigurationManager.AppSettings[name: "ItemGPI_ItemID"];

                queryString = "select CAST(LEFT(" + ItemGPI_GPI + ", "+ Parameters.gpiDigits + ") AS VARCHAR(50))," + PurchaseHistory_PurchaseDate + ", " + PurchaseHistory_PurchaseQuantity +
                                " from ((select * from " + PurchaseHistory_Table + ") a inner join ( select * from " + ItemGPI_Table + ") b on a." + PurchaseHistory_ItemID + 
                                "= b." + ItemGPI_ItemID + ") " + " where " + PurchaseHistory_CustomerID + "= @CustID and " + PurchaseHistory_PurchaseDate + "< @InvDate" +
                                " and " + ItemGPI_GPI + " is not null";

                distinctItems2 = Customer.GetAllItemsWithOUTGPIs(custNo);
                queryString2 = "select " + PurchaseHistory_ItemID + "," + PurchaseHistory_PurchaseDate + ", " + PurchaseHistory_PurchaseQuantity + " from " + PurchaseHistory_Table +
                                       " where " + PurchaseHistory_CustomerID + "= @CustID and " + PurchaseHistory_PurchaseDate + "< @InvDate";
            }
            
            List<string> ids = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@CustID", custNo);
                command.Parameters.AddWithValue("@InvDate", Parameters.processingDate);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (distinctItems.Contains((string)reader[0]))
                        {
                            if (ids.Contains((string)reader[0]))
                            {
                                var item = returnList.Find(x => x.Number == (string)reader[0]);
                                item.purchases.Add(new PNBDPurchaseData() { purchaseDate = (int)reader[1], purcaseQuantity = (int)reader[2] });
                            }
                            else
                            {
                                PNBDItemData item = new PNBDItemData() { Number = (string)reader[0], isGPI = Parameters.useGPI };
                                ids.Add((string)reader[0]);
                                item.purchases.Add(new PNBDPurchaseData() { purchaseDate = (int)reader[1], purcaseQuantity = (int)reader[2] });
                                returnList.Add(item);
                            }
                        }
                    }
                }
            }
            
            if (distinctItems2 != null)
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand(queryString2, connection);
                    command.Parameters.AddWithValue("@CustID", custNo);
                    command.Parameters.AddWithValue("@InvDate", Parameters.processingDate);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (distinctItems2.Contains((string)reader[0]))
                            {
                                if (ids.Contains((string)reader[0]))
                                {
                                    var item = returnList.Find(x => x.Number == (string)reader[0]);
                                    item.purchases.Add(new PNBDPurchaseData() { purchaseDate = (int)reader[1], purcaseQuantity = (int)reader[2] });
                                }
                                else
                                {
                                    PNBDItemData item = new PNBDItemData() { Number = (string)reader[0], isGPI = false };
                                    ids.Add((string)reader[0]);
                                    item.purchases.Add(new PNBDPurchaseData() { purchaseDate = (int)reader[1], purcaseQuantity = (int)reader[2] });
                                    returnList.Add(item);
                                }
                            }
                        }
                    }
                }
            }

            return returnList;
        }

        
        public ARIMAData ARIMAprepare(int date)
        {
            ARIMAData data = new ARIMAData();
            ARIMAData.date = date;
            ARIMACustomerData.preparer = this;

            List<string> customerIDs = ARIMAreadAllCustomers();

            foreach (var customer in customerIDs)
                data.AllCustomers.Add(new ARIMACustomerData() { Number = customer });

            return data;
        }
        public List<string> ARIMAreadAllCustomers()
        {
            List<string> allCustomers = new List<string>();

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
            string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];

            string queryString = "select distinct(" + CustomerID + ") from " + Table + " where " + PurchaseDate + "< @date and " +
                PurchaseDate + "> @dateMin";
           

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@date", ARIMAData.date);
                command.Parameters.AddWithValue("@dateMin",
                    DateManipulation.DateTimeToint(DateManipulation.intToDateTime(ARIMAData.date).AddMonths(-Parameters.customerRecency)));

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allCustomers.Add(reader[0].ToString());
                    }
                }
            }

            return allCustomers;
        }
        public List<ARIMAItemData> ARIMAreadAllItems(string custNo)
        {
            List<ARIMAItemData> returnData = new List<ARIMAItemData>();

            var items = new List<string>();
            if (!Parameters.useGPI)
                items = Customer.GetAllItems(custNo);
            else
                items = Customer.GetAllItemsWithOUTGPIs(custNo);

            foreach (var item in items)
            {
                ARIMAItemData itemData = new ARIMAItemData() { Number = item };
                itemData.StartDate = ARIMAgetStartDate(custNo, item);
                itemData.EndDate = ARIMAgetEndDate(custNo, item);
                lock (ARIMAData.thisLock)
                {
                    if (ARIMAData.AllItemData.Find(x => x.Number == item) == null)
                    {
                        int endDate = Parameters.processingDate;
                        int startDate = DateManipulation.DateTimeToint(DateManipulation.intToDateTime(Parameters.processingDate).AddYears(-2));
                        ARIMAData.AllItemData.Add(new ARIMAItemData() { Number = item, customerConsumption = ARIMAgetGlobalConsumption(item, startDate, endDate), EndDate = endDate, StartDate = startDate, IsGPI = false });
                    }
                }
                itemData.customerConsumption = ARIMAgetCustomerConsumption(custNo, item, itemData.StartDate, itemData.EndDate);
                returnData.Add(itemData);
            }

            if(Parameters.useGPI)
            {
                items = Customer.GetAllItemsWithGPIs(custNo);
                foreach (var gpi in items)
                {
                    ARIMAItemData itemData = new ARIMAItemData() { Number = gpi };
                    itemData.StartDate = ARIMAgetStartDate(custNo, gpi);
                    itemData.EndDate = ARIMAgetEndDate(custNo, gpi);
                    lock (ARIMAData.thisLock)
                    {
                        if (ARIMAData.AllItemData.Find(x => x.Number == gpi) == null)
                        {
                            int endDate = Parameters.processingDate;
                            int startDate = DateManipulation.DateTimeToint(DateManipulation.intToDateTime(Parameters.processingDate).AddYears(-2));
                            ARIMAData.AllItemData.Add(new ARIMAItemData() { Number = gpi, customerConsumption = ARIMAgetGlobalGPIConsumption(gpi, startDate, endDate), EndDate = endDate, StartDate = startDate, IsGPI = true });
                        }
                    }
                    itemData.customerConsumption = ARIMAgetCustomerConsumption(custNo, gpi, itemData.StartDate, itemData.EndDate);
                    returnData.Add(itemData);
                }
            }

            return returnData;
        }
        public int ARIMAgetStartDate(string custNo,string itemNo)
        {
            int start = -1; 

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
            string ItemID = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
            string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];

            string queryString = "select min(" + PurchaseDate + ") from " +
                Table + " where " + CustomerID + "=@custNo and " + ItemID + "=@itemNo and " + PurchaseDate + "<@Date";


            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@custNo", custNo);
                command.Parameters.AddWithValue("@itemNo", itemNo);
                command.Parameters.AddWithValue("@Date", Parameters.processingDate);


                connection.Open();

                using (var reader = command.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        start = (int)reader[0];
                    }

                }
            }
            return start;
        }
        public int ARIMAgetEndDate(string custNo, string itemNo)
        {
            int end = -1;

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
            string ItemID = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
            string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];

            string queryString = "select max(" + PurchaseDate + ") from " +
                Table + " where " + CustomerID + "=@custNo and " + ItemID + "=@itemNo and " + PurchaseDate + "<@Date";


            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@custNo", custNo);
                command.Parameters.AddWithValue("@itemNo", itemNo);
                command.Parameters.AddWithValue("@Date", Parameters.processingDate);


                connection.Open();

                using (var reader = command.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        end = (int)reader[0];
                    }

                }
            }
            return end;
        }
        public List<ARIMAConsumptionData> ARIMAgetGlobalConsumption(string itemNo, int start, int end)
        {
            List<ARIMAConsumptionData> Consumptions = new List<ARIMAConsumptionData>();

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "ItemConsumption"];
            string ItemID = ConfigurationManager.AppSettings[name: "ItemConsumption_ItemID"];
            string Date = ConfigurationManager.AppSettings[name: "ItemConsumption_Date"];
            string Consumption = ConfigurationManager.AppSettings[name: "ItemConsumption_Consumption"];

            string queryString = "select " + Consumption + ", " + Date + " from " + Table +
                                   " where " + ItemID + "= @ItemID and " + Date + "< @nextPurchase and " +
                                   Date + ">= @begin order by " + Date;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@ItemID", itemNo);
                command.Parameters.AddWithValue("@nextPurchase", Parameters.processingDate);
                command.Parameters.AddWithValue("@begin", start);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Consumptions.Add(new ARIMAConsumptionData { Date = (int)reader[1], Value = (double)reader[0] });
                    }
                }
            }
            DateTime stopedAsDate = DateManipulation.intToDateTime(Consumptions[Consumptions.Count - 1].Date).AddDays(1);
            int stopedAsInt = DateManipulation.DateTimeToint(stopedAsDate);
            
            double avg = Consumptions.Average(x => x.Value);

            while (stopedAsInt < Parameters.processingDate)
            {
                Consumptions.Add(new ARIMAConsumptionData { Date = stopedAsInt, Value = avg });

                stopedAsDate = stopedAsDate.AddDays(1);
                stopedAsInt = DateManipulation.DateTimeToint(stopedAsDate);
            }


            return Consumptions;
        }

        public List<ARIMAConsumptionData> ARIMAgetGlobalGPIConsumption(string GPINo, int start, int end)
        {
            List<ARIMAConsumptionData> quantity = new List<ARIMAConsumptionData>();

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string PurchaseHistory_Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
            string PurchaseHistory_ItemID = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
            string PurchaseHistory_PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];
            string PurchaseHistory_PurchaseQuantity = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseQuantity"];

            string ItemGPI_Table = ConfigurationManager.AppSettings[name: "ItemGPI"];
            string ItemGPI_GPI = ConfigurationManager.AppSettings[name: "ItemGPI_GPI"];
            string ItemGPI_ItemID = ConfigurationManager.AppSettings[name: "ItemGPI_ItemID"];

            string queryString = "select " + PurchaseHistory_PurchaseDate + ", " + PurchaseHistory_PurchaseQuantity + " from " + PurchaseHistory_Table +
                                   " where " + PurchaseHistory_PurchaseDate + "<= @end and" + PurchaseHistory_ItemID + " in "+
                                   "(select " + ItemGPI_ItemID + " from " + ItemGPI_Table + " where " + ItemGPI_GPI + " =@GPI )" +
                                   " order by " + PurchaseHistory_PurchaseDate;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@GPI", GPINo);
                command.Parameters.AddWithValue("@end", end);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        quantity.Add(new ARIMAConsumptionData { Date = (int)reader[0], Value = (int)reader[1] });
                    }
                }
            }

            List<ARIMAConsumptionData> quantityList = (from purchases in quantity
                                                       group purchases by purchases.Date into purchaseByDate
                                                       select new ARIMAConsumptionData { Date = purchaseByDate.Key, Value = purchaseByDate.Sum(x => x.Value) }).OrderBy(x => x.Date).ToList();

            quantityList.RemoveAll(x => x.Value == 0);

            List<ARIMAConsumptionData> Consumptions = new List<ARIMAConsumptionData>();
            Consumptions = TransformQuantityData(quantityList);

            DateTime stopedAsDate = DateManipulation.intToDateTime(Consumptions[Consumptions.Count - 1].Date).AddDays(1);
            int stopedAsInt = DateManipulation.DateTimeToint(stopedAsDate);

            double avg = Consumptions.Average(x => x.Value);

            while (stopedAsInt < Parameters.processingDate)
            {
                Consumptions.Add(new ARIMAConsumptionData { Date = stopedAsInt, Value = avg });

                stopedAsDate = stopedAsDate.AddDays(1);
                stopedAsInt = DateManipulation.DateTimeToint(stopedAsDate);
            }

            return Consumptions;
        }

        public List<ARIMAConsumptionData> ARIMAgetCustomerConsumption(string custNo,string itemNo, int start, int end)
        {
            List<ARIMAConsumptionData> quantity = new List<ARIMAConsumptionData>();
            int endMinus2Years = DateManipulation.DateTimeToint(DateManipulation.intToDateTime(end).AddYears(-2));

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
            string ItemID = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
            string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];
            string PurchaseQuantity = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseQuantity"];

            string queryString = "select " + PurchaseDate + ", " + PurchaseQuantity + " from " + Table +
                                   " where " + ItemID + "= @ItemID and " + CustomerID + "= @CustID and " +
                                   PurchaseDate + ">= @begin and " + PurchaseDate + "<= @end order by " + PurchaseDate;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@ItemID", itemNo);
                command.Parameters.AddWithValue("@CustID", custNo);
                command.Parameters.AddWithValue("@end", end);
                command.Parameters.AddWithValue("@begin", start > endMinus2Years ? start : endMinus2Years);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        quantity.Add(new ARIMAConsumptionData { Date = (int)reader[0], Value = (int)reader[1] });
                    }
                }
            }

            List<ARIMAConsumptionData> quantityList = (from purchases in quantity
                                                group purchases by purchases.Date into purchaseByDate
                                                select new ARIMAConsumptionData { Date = purchaseByDate.Key, Value = purchaseByDate.Sum(x => x.Value) }).OrderBy(x => x.Date).ToList();

            quantityList.RemoveAll(x => x.Value == 0);

            List<ARIMAConsumptionData> returnList = new List<ARIMAConsumptionData>();
            returnList = TransformQuantityData(quantityList);
            return returnList;
        }
        public List<ARIMAConsumptionData> TransformQuantityData(List<ARIMAConsumptionData> quantity)
        {
            List<ARIMAConsumptionData> returnList = new List<ARIMAConsumptionData>();
            DateTime current = DateManipulation.intToDateTime(quantity[0].Date);
            DateTime? next = DateManipulation.intToDateTime(quantity[1].Date);
            int daysBetween = ((DateTime)next - current).Days;
            int i = 0;
            int count = quantity.Count();

            while (next != null)
            {
                returnList.Add(new ARIMAConsumptionData() { Value = quantity[i].Value / daysBetween, Date = DateManipulation.DateTimeToint(current) });
                current = current.AddDays(1);
                if (current == next)
                {
                    ++i;
                    if (i + 1 < count)
                    {
                        next = DateManipulation.intToDateTime(quantity[i + 1].Date);
                        daysBetween = ((DateTime)next - current).Days;
                    }
                    else
                        next = null;
                }
            }

            return returnList;
        }

        public string GetScriptPath()
        {
            if (ScriptPath != null)
                return ScriptPath;

            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = dir + @"R\ARIMA\RscriptFull.r";
            ScriptPath = TempFileHelper.CreateTmpFile();
            using (var streamWriter = new StreamWriter(new FileStream(ScriptPath, FileMode.Open, FileAccess.Write)))
            {
                using (var streamReader = new StreamReader(new FileStream(filePath, FileMode.Open)))
                {
                    streamWriter.Write(streamReader.ReadToEnd());
                }

            }

            return ScriptPath;
        }
    }
}
