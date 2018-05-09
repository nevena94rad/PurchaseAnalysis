using Baza.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.GPISelector
{
    public class GPILastPurchased : I_GPISelector
    {
        public string displayName { get { return "Last purchased"; } }
        public string Select(string customer, string GPInumber)
        {
            string last = "";
            int? lastDate = null;

            string queryString;
            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            string PurchaseHistory_Table = ConfigurationManager.AppSettings[name: "PurchaseHistory"];
            string PurchaseHistory_CustomerID = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
            string PurchaseHistory_ItemID = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
            string PurchaseHistory_PurchaseDate = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];

            string ItemGPI_Table = ConfigurationManager.AppSettings[name: "ItemGPI"];
            string ItemGPI_GPI = ConfigurationManager.AppSettings[name: "ItemGPI_GPI"];
            string ItemGPI_ItemID = ConfigurationManager.AppSettings[name: "ItemGPI_ItemID"];

            queryString = "select a." + PurchaseHistory_ItemID + ", max(" + PurchaseHistory_PurchaseDate + ") from((select * from " + PurchaseHistory_Table +
                                ") a inner join ( select * from " + ItemGPI_Table + ") b on a." + PurchaseHistory_ItemID + "= b." + ItemGPI_ItemID + ") " + 
                                   " where " + PurchaseHistory_CustomerID + "= @CustID and " + PurchaseHistory_PurchaseDate + "< @InvDate and " +
                                   "CAST(LEFT(" + ItemGPI_GPI + ", "+ Parameters.gpiDigits+ ") AS VARCHAR(20))" + " =@itemGPI " +
                                   "group by a." + PurchaseHistory_ItemID;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@CustID", customer);
                command.Parameters.AddWithValue("@InvDate", Parameters.processingDate);
                command.Parameters.AddWithValue("@itemGPI", GPInumber.Length >= Parameters.gpiDigits ? GPInumber.Substring(0, Parameters.gpiDigits) : GPInumber);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    { 
                        var purchaseDate = Int32.Parse(reader[1].ToString());
                        var purchasedItem = reader[0].ToString();

                        if (lastDate == null || (lastDate < purchaseDate))
                        {
                            last = purchasedItem;
                            lastDate = purchaseDate;
                        }
                    }
                }
            }

            return last;
        }
    }
}
