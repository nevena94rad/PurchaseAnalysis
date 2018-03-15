using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Baza.DTO
{
    public static class Parameters
    {
        public static int customerRecency = 6;
        public static double predictionPercentageCutOff = 10;
        public static int predictionCountCutOff = 10;
        public static int processingDate;
        public static int ID;

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
            string ProcessingStart = ConfigurationManager.AppSettings[name: "Parameters_ProcessingStart"];
            string ProcessingParameters = ConfigurationManager.AppSettings[name: "Parameters_ProcessingParameters"];

            string queryString = "insert into " + Table + "(" + ProcessingStart + "," + ProcessingParameters + ") OUTPUT INSERTED.ID values (@ProcessingStart, @Parameters) ";
            queryString += @"SELECT SCOPE_IDENTITY();";

            DateTime processingStart = DateTime.Now;

            string jsonParameters = string.Empty;
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("customerRecency", customerRecency.ToString());
            values.Add("predictionPercentageCutOff", predictionPercentageCutOff.ToString());
            values.Add("predictionCountCutOff", predictionCountCutOff.ToString());
            values.Add("processingDate", processingDate.ToString());
            jsonParameters = JsonConvert.SerializeObject(values);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@ProcessingStart", processingStart);
                command.Parameters.AddWithValue("@Parameters", jsonParameters);

                ID = Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}
