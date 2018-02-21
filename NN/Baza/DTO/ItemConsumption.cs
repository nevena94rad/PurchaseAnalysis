using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public class ItemConsumption
    {
        public string itemNo;
        public int startDate;
        public List<double> listOfValues;

        public static void readAllItemData(int nextPurchase)
        {
            List<string> allItems = new List<string>();

            var db = new DataClasses1DataContext();

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            string Table = ConfigurationManager.AppSettings[name: "ItemConsumption"];
            string ItemID = ConfigurationManager.AppSettings[name: "ItemConsumption_ItemID"];
            string Consumption = ConfigurationManager.AppSettings[name: "ItemConsumption_Consumption"];
            string Date = ConfigurationManager.AppSettings[name: "ItemConsumption_Date"];

            string queryString = "select distinct( " + ItemID + ") from " + Table +" where " + Date +" < @nextPurchase";

            string queryString1 = "select " + Consumption + " from " + Table +
                " where " + Date + " < @nextPurchase and " + ItemID + "=@itemID order by "+Date;

            string queryString2 = "select min(" + Date + ") from " + Table +
                " where " + Date + " < @nextPurchase and " + ItemID + "=@itemID";

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@nextPurchase", nextPurchase);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        allItems.Add((String)reader[0]);
                    }
                }

                foreach (var item in allItems)
                {
                    List<double> Consumptions = new List<double>();
                    int first = -1;
                    var command1 = new SqlCommand(queryString1, connection);
                    command1.Parameters.AddWithValue("@nextPurchase", nextPurchase);
                    command1.Parameters.AddWithValue("@itemID", item);
                    var command2 = new SqlCommand(queryString2, connection);
                    command2.Parameters.AddWithValue("@nextPurchase", nextPurchase);
                    command2.Parameters.AddWithValue("@itemID", item);

                    using (var reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Consumptions.Add((double)reader[0]);
                        }
                    }
                    using (var reader = command2.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            first = (int)reader[0];
                        }
                    }
                    if(first!= -1)
                        ConsumptionData.Instance.addData(new ItemConsumption() { itemNo = item, listOfValues = Consumptions, startDate = first });
                }
            }
        }
    }
}
