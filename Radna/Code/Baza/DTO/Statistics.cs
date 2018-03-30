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
        public List<Purchase> predictedPurchases = new List<Purchase>();
        public List<Purchase> occuredPurchases = new List<Purchase>();

        protected bool allPurchases { get; set; }
        public int predictionCount { get { return predictedPurchases.Count(); } }
        public double correctPredictionsPercentage { get { return 100 * (((double)correctPredictionsCount) / (predictionCount > 0 ? predictionCount : -1)); } }
        public int falsePredictionCount { get { return predictionCount - correctPredictionsCount; } }
        public double falsePredictionPercentage { get { return 100 - correctPredictionsPercentage; } }
        public double coveragePercentage { get { return 100 * (occuredPurchases.Count > 0 ? (((double)correctPredictionsCount) / occuredPurchases.Count()) : -1); } }

        public int correctPredictionsCount { get; protected set; }
        public int startingDate { get; protected set; }
        

        protected Statistics(int date, bool allPurchases)
        {
            startingDate = date;
            this.allPurchases = allPurchases;
        }
        protected void getStatistics()
        {
            getPredictedPurchases();
            getOccuredPurchases();
            setCorrectPredictionCount();     
        }
        protected void setCorrectPredictionCount()
        {
            PurchaseComperer comperer = new PurchaseComperer();
            correctPredictionsCount = predictedPurchases.Intersect(occuredPurchases, comperer).ToList().Count;
        }
        protected void getOccuredPurchases()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            string HistoryTable = ConfigurationManager.AppSettings[name: "PurchaseHistory"];

            string History_CustNumber = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
            string History_ItemNumber = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
            string History_Date = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];

            string query = "";

            if (allPurchases)
                query = "select distinct " + History_ItemNumber + ", " + History_CustNumber + " from " + HistoryTable +
                            " where " + History_Date + ">=@startingDate and " + History_Date + "<=@endDate order by CustNo";
            else
                query = "select distinct " + History_ItemNumber + ", " + History_CustNumber + " from " + HistoryTable +
                        " where " + History_Date + "<=@endDate group by " + History_ItemNumber + ", " + History_CustNumber +
                        "having count(*)>2 and max(" + History_Date + ") >=@startingDate"; 

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

        public static List<Purchase> getListOfPurchases(string custNo, string itemNo)
        {
            List<Purchase> returnList = new List<Purchase>();

            var connectionString = ConfigurationManager.ConnectionStrings[name: "PED"].ConnectionString;

            string HistoryTable = ConfigurationManager.AppSettings[name: "PurchaseHistory"];

            string History_CustNumber = ConfigurationManager.AppSettings[name: "PurchaseHistory_CustomerID"];
            string History_ItemNumber = ConfigurationManager.AppSettings[name: "PurchaseHistory_ItemID"];
            string History_Date = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseDate"];
            string History_Quantity = ConfigurationManager.AppSettings[name: "PurchaseHistory_PurchaseQuantity"];


            string query = "select " + History_ItemNumber + ", " + History_CustNumber +
                            ", " + History_Date + ", " + History_Quantity + " from " + HistoryTable +
                            " where " + History_ItemNumber + "=@itemNo and " +
                            History_CustNumber + "=@custNo order by " + History_Date;


            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@itemNo", itemNo);
                command.Parameters.AddWithValue("@custNo", custNo);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        returnList.Add(new Purchase((string)reader[0], (string)reader[1],DateManipulation.intToDateTime((int)reader[2]), (int)reader[3]));
                    }
                }
            }


            return returnList;
        }

        protected abstract void getPredictedPurchases();
        
    }

    public class NEWStatistics : Statistics
    {
        protected int parametarID;
        protected double cutOffPercentage;
        
        public NEWStatistics(int parametarID, int date, bool allPurchases, double cutOffPercentage) : base(date, allPurchases)
        {
            this.parametarID = parametarID;
            this.cutOffPercentage = cutOffPercentage;
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
            string Prediction_PredictedValue = ConfigurationManager.AppSettings[name: "PurchasePrediction_ProcessingValue"];


            string Model_ID = ConfigurationManager.AppSettings[name: "CustomerModel_ID"];
            string Model_ParameterID = ConfigurationManager.AppSettings[name: "CustomerModel_Parameters_ID"];

            string query = "select " + Prediction_ItemNumber + ", " + Prediction_CustNumber + " from " + PredictionTable +
                           " where " + Prediction_ModelID + " in ( " +
                                "select " + Model_ID + " from " + ModelTable +
                                " where " + Model_ParameterID + " =@parametarID )" + 
                           " and " + Prediction_PredictedValue + " >=" + cutOffPercentage;
                           

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
        public OLDStatistics(int date, bool allPurchases) : base(date, allPurchases)
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
                            " where " + Recomend_Date + ">=@startingDate and " + Recomend_Date + "<=@endDate order by CustNo";

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
