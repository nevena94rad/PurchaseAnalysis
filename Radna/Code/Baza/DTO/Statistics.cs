using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public abstract class Statistics
    {
        protected List<Purchase> predictedPurchases = new List<Purchase>();
        protected List<Purchase> occuredPurchases = new List<Purchase>();


        public int predictionCount { get { return predictedPurchases.Count(); } }
        public double correctPredictionsPercentage { get { return 100 * (((double)correctPredictionsCount) / predictionCount > 0 ? predictionCount : -1); } }
        public int falsePredictionCount { get { return predictionCount - correctPredictionsCount; } }
        public double falsePredictionPercentage { get { return 100 - correctPredictionsPercentage; } }
        public double coveragePercentage { get { return occuredPurchases.Count > 0 ? (((double)correctPredictionsCount) / occuredPurchases.Count()) : -1; } }

        public int correctPredictionsCount { get; protected set; }
        public int startingDate { get; protected set; }

        protected Statistics(int date)
        {
            startingDate = date;
        }
        protected void getStatistics()
        {
            getPredictedPurchases();
            getOccuredPurchases();
            setCorrectPredictionCount();     
        }
        protected void setCorrectPredictionCount()
        {
            correctPredictionsCount = predictedPurchases.Intersect(occuredPurchases).Count();
        }
        protected void getOccuredPurchases()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            string HistoryTable = ConfigurationManager.AppSettings[name: "PurchaseHistory"];

            string History_CustNumber = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
            string History_ItemNumber = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
            string History_Date = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];

            string query = "select distinct " + History_ItemNumber + ", " + History_CustNumber + " from " + HistoryTable +
                            "where " + History_Date + ">=@startingDate and " + History_Date + "<=endDate";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@startingDate", startingDate);
                command.Parameters.AddWithValue("@endDate", DateManipulation.DateTimeToint(DateManipulation.intToDateTime((int)startingDate).AddDays(6)));

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        occuredPurchases.Add(new Purchase((string)reader[0], (string)reader[1]));
                    }
                }
            }

        }

        protected abstract void getPredictedPurchases();
        
    }

    public class NEWStatistics : Statistics
    {
        protected int parametarID;
        
        public NEWStatistics(int parametarID, int date) : base(date)
        {
            this.parametarID = parametarID;
            getStatistics();
        }
        protected override void getPredictedPurchases()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            string PredictionTable = ConfigurationManager.AppSettings[name: "PurchasePrediction"];
            string ModelTable = ConfigurationManager.AppSettings[name: "CustomerModel"];

            string Prediction_CustNumber = ConfigurationManager.AppSettings[name: "PurchasePrediction_CustomerID"];
            string Prediction_ItemNumber = ConfigurationManager.AppSettings[name: "PurchasePrediction_ItemID"];
            string Prediction_ModelID = ConfigurationManager.AppSettings[name: "PurchasePrediction_ModelID"];

            string Model_ID = ConfigurationManager.AppSettings[name: "CustomerModel_ID"];
            string Model_ParameterID = ConfigurationManager.AppSettings[name: "CustomerModel_Parameters_ID"];

            string query = "select " + Prediction_ItemNumber + ", " + Prediction_CustNumber + " from " + PredictionTable +
                           "where " + Prediction_ModelID + " in ( " +
                                "select " + Model_ID + " from " + ModelTable +
                                "where " + Model_ParameterID + " =@parametarID )";
                           

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@parametarID", parametarID);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        predictedPurchases.Add(new Purchase((string)reader[0], (string)reader[1]));
                    }
                }
            }
        }
        
    }
    public class OLDStatistics : Statistics
    {
        public OLDStatistics(int date) : base(date)
        {
            getStatistics();
        }
        protected override void getPredictedPurchases()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            string RecomendTable = ConfigurationManager.AppSettings[name: "PLS_RecomendHist"];

            string Recomend_CustNumber = ConfigurationManager.AppSettings[name: "PLS_RecomendHist_CustNo"];
            string Recomend_ItemNumber = ConfigurationManager.AppSettings[name: "PLS_RecomendHist_ItemNo"];
            string Recomend_Date = ConfigurationManager.AppSettings[name: "PLS_RecomendHist_ProcessingDateInt"];

            string query = "select distinct " + Recomend_ItemNumber + ", " + Recomend_CustNumber + " from " + RecomendTable +
                            "where " + Recomend_Date + ">=@startingDate and " + Recomend_Date + "<=endDate";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@startingDate", startingDate);
                command.Parameters.AddWithValue("@endDate", DateManipulation.DateTimeToint(DateManipulation.intToDateTime((int)startingDate).AddDays(6)));

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        predictedPurchases.Add(new Purchase((string)reader[0], (string)reader[1]));
                    }
                }
            }
        }
    }
}
