using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public class GPI
    {
        public string ItemNo { get; set; }
        public string GPINumber { get; set; }

        static public int getMaxNumOfDigits()
        {
            int maxNum = 0;

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "ItemGPI"];
            string GPI = ConfigurationManager.AppSettings[name: "ItemGPI_GPI"];

            string queryString = "select top 1 LEN(" + GPI + ") itemGPI from " + Table + " where " + GPI + " IS NOT NULL order by itemGPI ASC";

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        maxNum = (int)reader[0];
                    }
                }
            }
            return maxNum;
        }
    }
}
