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
    public class SimplePrepare : PNBDPrepare
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

       
    }
}
