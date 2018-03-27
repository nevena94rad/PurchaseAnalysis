using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public class Purchase : IEquatable<Purchase>
    {
        public string ItemNo { get; set; }
        public string CustNo { get; set; }

        public Purchase(string itemNo, string custNo)
        {
            ItemNo = itemNo;
            CustNo = custNo;
        }
        public bool Equals(Purchase other)
        {
            if (this.ItemNo == other.ItemNo && this.CustNo == other.CustNo)
                return true;
            else
                return false;
        }
    }
}
