using Baza.Prepare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Data
{
    public class PNBDCustomerData
    {
        protected List<PNBDItemData> items = null;
        public static PNBDPrepare preparer = null;

        public string Number { get; set; }
        public int modelID { get; set; }
        public List<PNBDItemData> itemPurchased
        {
            get
            {
                if (preparer == null)
                    return null;
                else if (items == null)
                    items = preparer.PNBDreadAllItems(Number);
                return items;
            }
            set
            {
                items = value;
            }
        } 
        public int NoPurchases { get { return itemPurchased.Sum(x => x.purchases.Count()); } }
    }
}
