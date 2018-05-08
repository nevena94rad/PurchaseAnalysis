using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
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
        public static bool useGPI = false;

        private static ILog log = LogManager.GetLogger(typeof(Parameters));

        public static void LoadParameters(int date, int recency, string percentage, string count, string calculator, string preparer)
        {
            if (recency > 0)
                customerRecency = recency;

            if (percentage != "")
                predictionPercentageCutOff = Double.Parse(percentage);

            if (count != "")
                predictionCountCutOff = Int32.Parse(count);

            processingDate = date;

            InsertIntoDatabase(calculator, preparer);
        }

        public static void InsertIntoDatabase(string calculator, string preparer)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "Parameters"];
            string ProcessingStart = ConfigurationManager.AppSettings[name: "Parameters_ProcessingStart"];
            string ProcessingParameters = ConfigurationManager.AppSettings[name: "Parameters_ProcessingParameters"];
            string ProcessingCalculator = ConfigurationManager.AppSettings[name: "Parameters_ProcessingCalculator"];
            string ProcessingPreparer = ConfigurationManager.AppSettings[name: "Parameters_ProcessingPreparer"];

            string queryString = "insert into " + Table + "(" + ProcessingStart + "," + ProcessingParameters + ", " + ProcessingCalculator + ", " + ProcessingPreparer +
                ") OUTPUT INSERTED.ID values (@ProcessingStart , @Parameters , @Calculator , @Preparer) ";
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
                command.Parameters.AddWithValue("@Calculator", calculator);
                command.Parameters.AddWithValue("@Preparer", preparer);

                ID = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public static void Update(int procStatus, string procError)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "Parameters"];
            string ProcessingEnd = ConfigurationManager.AppSettings[name: "Parameters_ProcessingEnd"];
            string ProcessingStatus = ConfigurationManager.AppSettings[name: "Parameters_ProcessingStatus"];
            string ProcessingError = ConfigurationManager.AppSettings[name: "Parameters_ProcessingError"];
            string Id = ConfigurationManager.AppSettings[name: "Parameters_ID"];

            string queryString = "UPDATE " + Table + " SET " + ProcessingEnd + "= @ProcessingEnd , " + ProcessingStatus + "= @ProcessingStatus , " + ProcessingError + "= @ProcessingError "+
                "WHERE " + Id + "= @Id;";

            DateTime processingEnd = DateTime.Now;
            string processingError = "";
            if(procStatus != (int)Enum.ProcessingStatus.Status.ERROR)
            {
                processingError = "NO ERROR";
            }
            else
            {
                processingError = procError;
            }
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@ProcessingEnd", processingEnd);
                command.Parameters.AddWithValue("@ProcessingStatus", ((Enum.ProcessingStatus.Status) procStatus).ToString());
                command.Parameters.AddWithValue("@ProcessingError", processingError);
                command.Parameters.AddWithValue("@ID", ID);

                command.ExecuteNonQuery();
            }
        }

        public static List<int> ParametersIDs(DateTime processingDate)
        {
            List<int> ids = new List<int>();
            int date = DateManipulation.DateTimeToint(processingDate);

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "Parameters"];
            string ProcessingParameters = ConfigurationManager.AppSettings[name: "Parameters_ProcessingParameters"];
            string ID = ConfigurationManager.AppSettings[name: "Parameters_ID"];

            string query = "select " + ID + " from " + Table +" where JSON_VALUE(" + ProcessingParameters + ",'$.processingDate') = @date";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@date", date);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ids.Add((int)reader[0]);
                    }
                }
            }

            return ids;
        }

        public static Dictionary<string,string> GetParameters(int parametersId, out string status)
        {
            string jsonParameters = "";
            status = "";
            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
            string Table = ConfigurationManager.AppSettings[name: "Parameters"];
            string ID = ConfigurationManager.AppSettings[name: "Parameters_ID"];
            string ProcessingParameters = ConfigurationManager.AppSettings[name: "Parameters_ProcessingParameters"];
            string ProcessingStatus = ConfigurationManager.AppSettings[name: "Parameters_ProcessingStatus"];
            string ProcessingCalculator = ConfigurationManager.AppSettings[name: "Parameters_ProcessingCalculator"];
            string ProcessingPreparer = ConfigurationManager.AppSettings[name: "Parameters_ProcessingPreparer"];

            string queryString = "select " + ProcessingParameters + "," + ProcessingStatus + " from " + Table + " where " + ID + "=@ID";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@ID", parametersId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        jsonParameters = (string)(reader[0]);

                        try
                        {
                            if (reader[1] != null)
                                status = (string)(reader[1]);
                            else
                                status = null;
                        }
                        catch(Exception e)
                        {
                            log.Error("Read non compleate run");
                        }
                    }
                }
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters = JsonConvert.DeserializeObject<Dictionary<string,string>>(jsonParameters);

            string queryString2 = "select " + ProcessingCalculator + ", " + ProcessingPreparer + " from " + Table + " where " + ID + "=@ID";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(queryString2, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@ID", parametersId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        try
                        {
                            if (reader[0] != null)
                                parameters.Add("calculator", (string)reader[0]);
                            if (reader[1] != null)
                                parameters.Add("preparer", (string)reader[1]);
                        }
                        catch (Exception e)
                        {
                            log.Error("Read pre model era");
                        }
                    }
                }
            }
            return parameters;
        }

        public static List<DateTime> GetProcessingDates()
        {
            List<DateTime> dates = new List<DateTime>();
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;
                string Table = ConfigurationManager.AppSettings[name: "Parameters"];
                string ProcessingParameters = ConfigurationManager.AppSettings[name: "Parameters_ProcessingParameters"];

                string query = "select distinct JSON_VALUE(" + ProcessingParameters + " ,'$.processingDate') from " + Table;

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand(query, connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dates.Add(DateManipulation.intToDateTime(Int32.Parse(reader[0].ToString())));
                        }
                    }
                }

                return dates;
            }
            catch(Exception ex)
            {
                return dates;
            }
        }
    }
}
