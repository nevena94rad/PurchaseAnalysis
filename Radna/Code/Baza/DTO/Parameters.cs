using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public static class Parameters
    {
        public static int customerRecency = 6;
        public static double predictionPercentageCutOff = 10;
        public static int predictionCountCutOff = 10;
        public static int processingDate;
        public static int tableID;

        public static void LoadParameters(int date, int recency, string percentage, string count)
        {
            if (recency > 0)
                customerRecency = recency;

            if (percentage != "")
                predictionPercentageCutOff = Double.Parse(percentage);

            if (count != "")
                predictionCountCutOff = Int32.Parse(count);

            processingDate = date;

            InsertIntoDatabase();
        }

        public static void InsertIntoDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "Parameters"];
            string Date = ConfigurationManager.AppSettings[name: "Parameters_ProcessingDate"];
            string CustRecency = ConfigurationManager.AppSettings[name: "Parameters_CustRecency"];
            string PercentageCutOff = ConfigurationManager.AppSettings[name: "Parameters_PercentageCutOff"];
            string CountCutOff = ConfigurationManager.AppSettings[name: "Parameters_CountCutOff"];

            string queryString = "insert into " + Table + "(" + Date + "," + CustRecency + "," + PercentageCutOff + ","
                + CountCutOff + ") OUTPUT INSERTED.ID values (@Date, @CustRecency, @PercentageCutOff, @CountCutOff) ";
            queryString += @"SELECT SCOPE_IDENTITY();";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Date", processingDate);
                command.Parameters.AddWithValue("@CustRecency", customerRecency);
                command.Parameters.AddWithValue("@PercentageCutOff", predictionPercentageCutOff);
                command.Parameters.AddWithValue("@CountCutOff", predictionCountCutOff);

                tableID = Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}
