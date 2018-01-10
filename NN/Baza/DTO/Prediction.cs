using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public class Prediction
    {
        public string itemNo;
        public int from;
        public int to;
        public int occurred;
        public double predictedConsumption;
        public int lastInvQty;
        public static int succsess = 0;
        public static int fail = 0;

        public static Prediction makePrediction(string customer, string item, int begin, int end, int nextPurchase, int lastInvQty)
        {
            // na osnovu parametara potrebno je popuniti polja
            // posmatra se period begin do end i predvidja se kad ce sledeca kupovina da bude
           
            var db = new DataClasses1DataContext();

            Prediction pred = new Prediction();
            pred.itemNo = item;
            pred.from = begin;
            pred.to = end;
            pred.occurred = nextPurchase;
            pred.lastInvQty = lastInvQty;

            string connectionString = db.Connection.ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = new SqlCommand("Forecast", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandTimeout = 3000000;

            command.Parameters.Add(new SqlParameter("@param1", item));
            command.Parameters.Add(new SqlParameter("@param2", customer));
            command.Parameters.Add(new SqlParameter("@param3", begin));
            command.Parameters.Add(new SqlParameter("@param4", end));
            command.Parameters.Add(new SqlParameter("@param5", nextPurchase));
            

            DataTable dt = new DataTable();
            try
            {
                dt.Load(command.ExecuteReader());
                pred.predictedConsumption = Convert.ToDouble(dt.Rows[0]["Potrosnja"]);
                succsess++;

            }
            catch(Exception e)
            {
                fail++;
                pred.predictedConsumption = -1;
            }

            connection.Close();
            connection.Dispose();
           

            return pred;
        }

        public static Prediction makePredictionAlternativeWay(string customer, string item, int begin, int end, int nextPurchase, int lastInvQty)
        {
            var db = new DataClasses1DataContext();

            var itemConsumption = (from purchases in db.ItemConsumptions
                                   where purchases.ItemNo == item && purchases.Date < nextPurchase
                                   && purchases.Date >= begin
                                   select new { purchases.Consumption, purchases.Date }).OrderBy(x => x.Date);

            var customerConsumption = (from purchases in db.PurchaseHistories
                                      where purchases.CustNo == customer && purchases.ItemNo == item
                                      && purchases.InvDate < nextPurchase && purchases.InvDate >= begin
                                      group purchases by purchases.InvDate into purchaseByDate
                                      select new { Date = purchaseByDate.Key, Qty = purchaseByDate.Sum(x => x.InvQty) }).OrderBy(x=>x.Qty);





             throw new Exception();
        }

        public static List<double> TransformConsumption(List<double> purchases)
        {
            List<double> returnList = new List<double>();



            return returnList;
        }

            public double getError()
        {
            return Math.Abs(1 - predictedConsumption / lastInvQty);
        }
    }
}
