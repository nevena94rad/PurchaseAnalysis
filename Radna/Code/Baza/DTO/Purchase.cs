using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public class Purchase
    {
        public string ItemNo { get; set; }
        public string CustNo { get; set; }

        public Purchase(string itemNo, string custNo)
        {
            ItemNo = itemNo;
            CustNo = custNo;
        }
    }
    public class PurchaseComperer : IEqualityComparer<Purchase>
    {
        public bool Equals(Purchase x, Purchase y)
        {
            if (x.ItemNo == y.ItemNo && x.CustNo == y.CustNo)
                return true;
            else
                return true;
        }

        public int GetHashCode(Purchase obj)
        {
            return (obj.CustNo + obj.ItemNo).GetHashCode();
        }
    }
}
