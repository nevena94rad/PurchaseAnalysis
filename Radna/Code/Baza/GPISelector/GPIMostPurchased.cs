using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.GPISelector
{
    public class GPIMostPurchased : I_GPISelector
    {
        public string displayName { get { return "Most purchased"; } }
        public string Select(string customer, string GPInumber)
        {
            return "";
        }
    }
}
