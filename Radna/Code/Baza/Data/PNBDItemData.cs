using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Data
{
    public class PNBDItemData
    {
        public bool isGPI { get; set; } = false;
        public string Number { get; set; }
        public List<PNBDPurchaseData> purchases { get; set; }
        public int LastPurchase { get { return purchases.Max(x => x.purchaseDate); } }

        public PNBDItemData()
        {
            purchases = new List<PNBDPurchaseData>();
        }
    }
    
}
