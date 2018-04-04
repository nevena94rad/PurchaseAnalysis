using Baza.Prepare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Data
{
    public class ARIMACustomerData
    {
        protected List<ARIMAItemData> items = null;
        public static ARIMAPrepare preparer = null;

        public string Number { get; set; }
        public List<ARIMAItemData> itemPurchased
        {
            get
            {
                if (preparer == null)
                    return null;
                else if (items == null)
                    items = preparer.ARIMAreadAllItems(Number);
                return items;
            }
            set
            {
                items = value;
            }
        }
    }
}
