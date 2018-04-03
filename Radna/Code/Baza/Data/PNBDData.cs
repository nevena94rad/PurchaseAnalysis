using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Data
{
    public class PNBDData
    {
        public static int date;
        public List<PNBDCustomerData> AllCustomers { get; set; }   
        
        public PNBDData()
        {
            AllCustomers = new List<PNBDCustomerData>();
        }
    }
}
