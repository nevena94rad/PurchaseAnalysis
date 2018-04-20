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
        public static Object thisLock = new Object();

        public List<ARIMACustomerData> AllCustomers {get; set; }
        public static List<ARIMAItemData> AllItemData { get; set; }

        public ARIMAData()
        {
            AllItemData = new List<ARIMAItemData>();
            AllCustomers = new List<ARIMACustomerData>();
        }
    }
}
