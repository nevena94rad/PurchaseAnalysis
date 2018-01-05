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
        public int ordersInbetween;

        public static Prediction makePrediction(string customer,string item,int begin,int end, int nextPurchase, int lastInvQty)
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

            command.Parameters.Add(new SqlParameter("@param1", "10-4784"));
            command.Parameters.Add(new SqlParameter("@param2", "FIV108"));
            command.Parameters.Add(new SqlParameter("@param3", 20150816));
            command.Parameters.Add(new SqlParameter("@param4", 20170425));
            command.Parameters.Add(new SqlParameter("@param5", 20171005));

            

            DataTable dt = new DataTable();

            dt.Load(command.ExecuteReader());
            return pred;
        }
    }
}
