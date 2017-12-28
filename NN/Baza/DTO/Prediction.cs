using System;
using System.Collections.Generic;
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
        public DateTime predicted;
        public DateTime occurred;
        public int ordersInbetween;

        public static Prediction makePrediction(string customer,string item,DateTime begin,DateTime end )
        {
            // na osnovu parametara potrebno je popuniti polja
            // posmatra se period begin do end i predvidja se kad ce sledeca kupovina da bude
            throw new Exception();
        }
    }
}
