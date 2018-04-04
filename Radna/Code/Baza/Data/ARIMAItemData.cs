using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Data
{
    public class ARIMAItemData
    {
        public string Number { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }
        public List<ARIMAConsumptionData> consumption { get; set; }
        public List<ARIMAQuantityData> quantity { get; set; }

        public ARIMAItemData()
        {
            consumption = new List<ARIMAConsumptionData>();
            quantity = new List<ARIMAQuantityData>();
        }
    }
}
