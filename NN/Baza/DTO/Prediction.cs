using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public class Prediction
    {
        public string itemNo;
        public DateTime from;
        public DateTime to;
        public DateTime occurred;
        public double predictedConsumption;
        public int lastInvQty;
        public int ordersInbetween;

        public static Prediction makePrediction(string customer,string item,int begin, int end, int nextPurchase, int lastInvQty)
        {
            // na osnovu parametara potrebno je popuniti polja
            // posmatra se period begin do end i predvidja se kad ce sledeca kupovina da bude
            throw new Exception();
        }
        public double getError()
        {
            return Math.Abs(1 - predictedConsumption / lastInvQty);
        }
    }
}
