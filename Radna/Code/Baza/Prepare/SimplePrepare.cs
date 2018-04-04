using Baza.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baza.DTO;
using System.Configuration;
using System.Data.SqlClient;

namespace Baza.Prepare
{
    public class SimplePrepare : PNBDPrepare ,ARIMAPrepare
    {
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
            var distinctItems = Customer.GetAllItems(custNo);

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
            string CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
            string ItemID = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
            string PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];
            string PurchaseQuantity = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseQuantity"];

            string queryString = "select " + ItemID + "," + PurchaseDate + ", " + PurchaseQuantity + " from " + Table +
                                   " where " + CustomerID + "= @CustID and " + PurchaseDate + "< @InvDate";

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
                                PNBDItemData item = new PNBDItemData() { Number = (string)reader[0] };
                                ids.Add((string)reader[0]);
                                item.purchases.Add(new PNBDPurchaseData() { purchaseDate = (int)reader[1], purcaseQuantity = (int)reader[2] });
                                returnList.Add(item);
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

        public List<ARIMAItemData> ARIMAreadAllItems(string custNo)
        {
            List<ARIMAItemData> returnData = new List<ARIMAItemData>();

            var items = Customer.GetAllItems(custNo);

            foreach (var item in items)
            {
                ARIMAItemData itemData = new ARIMAItemData() { Number = item };
                itemData.StartDate = ARIMAgetStartDate(custNo, item);
                itemData.EndDate = ARIMAgetEndDate(custNo, item);
                itemData.consumption = ARIMAgetConsumption(item,itemData.StartDate);
                itemData.quantity = ARIMAgetQuantity(custNo, item, itemData.StartDate, itemData.EndDate);
                returnData.Add(itemData);
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

        public List<ARIMAConsumptionData> ARIMAgetConsumption(string itemNo,int start)
        {
            List<ARIMAConsumptionData> Consumptions = new List<ARIMAConsumptionData>();

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "ItemConsumptions"];
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
                        Consumptions.Add(new ARIMAConsumptionData { Date = (int)reader[0], Value = (int)reader[1] });
                    }
                }
            }

            return Consumptions;
        }

        public List<ARIMAQuantityData> ARIMAgetQuantity(string custNo,string itemNo, int start, int end)
        {
            List<ARIMAQuantityData> quantity = new List<ARIMAQuantityData>();

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
                command.Parameters.AddWithValue("@begin", start);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        quantity.Add(new ARIMAQuantityData { Date = (int)reader[0], Value = (int)reader[1] });
                    }
                }
            }

            List<ARIMAQuantityData> quantityList = (from purchases in quantity
                                                group purchases by purchases.Date into purchaseByDate
                                                select new ARIMAQuantityData { Date = purchaseByDate.Key, Value = purchaseByDate.Sum(x => x.Value) }).OrderBy(x => x.Date).ToList();

            quantity.RemoveAll(x => x.Value == 0);

            List<ARIMAQuantityData> returnList = new List<ARIMAQuantityData>();
            returnList = ARIMAQuantityData.TransformQuantityData(quantity);
            return returnList;
        }
    }
}
