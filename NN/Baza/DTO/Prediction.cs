using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

            SqlCommand command = new SqlCommand("Forecast", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@param1", item));
            command.Parameters.Add(new SqlParameter("@param2", customer));
            command.Parameters.Add(new SqlParameter("@param3", begin));
            command.Parameters.Add(new SqlParameter("@param4", end));
            command.Parameters.Add(new SqlParameter("@param5", nextPurchase));

            connection.Open();

            DataTable dt = new DataTable();

            dt.Load(command.ExecuteReader());
            GridView1.DataSource = dt;
            gvGrid.DataBind();
        }
    }
}
