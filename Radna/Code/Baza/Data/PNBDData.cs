using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Data
{
    public class PNBDData
    {
        public List<string> AllCustomers { get; set; }
        public List<string> AllItems { get; set; }
        public List<int> LastPurchases { get; set; }
        public int modelID { get; set; }
    }
}
