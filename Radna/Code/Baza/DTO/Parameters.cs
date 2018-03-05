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

        public static void LoadParameters(int date)
        {
            string cR = ConfigurationManager.AppSettings[name: "customerRecency"];
            if (cR != null)
                customerRecency = Int32.Parse(cR);

            string ppCO = ConfigurationManager.AppSettings[name: "predictionPercentageCutOff"];
            if (ppCO != null)
                predictionPercentageCutOff = Double.Parse(ppCO);

            string pCCO = ConfigurationManager.AppSettings[name: "predictionCountCutOff"];
            if (pCCO != null)
                predictionCountCutOff = Int32.Parse(pCCO);

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
                + CountCutOff + ") OUTPUT Inserted.key values (@Date, @CustRecency, @PercentageCutOff, @CountCutOff)";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@Date", processingDate);
                command.Parameters.AddWithValue("@CustRecency", customerRecency);
                command.Parameters.AddWithValue("@PercentageCutOff", predictionPercentageCutOff);
                command.Parameters.AddWithValue("@CountCutOff", predictionCountCutOff);

                tableID = (int) command.ExecuteScalar();

                
            }
            
        }
    }
}
