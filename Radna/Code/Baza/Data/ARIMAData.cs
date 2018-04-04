using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Data
{
    public class ARIMAData
    {
        public static int date;
        public List<ARIMACustomerData> AllCustomers { get; set; }

        public ARIMAData()
        {
            AllCustomers = new List<ARIMACustomerData>();
        }
    }
}
