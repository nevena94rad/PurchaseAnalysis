using Baza.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Prepare
{
    public interface I_PNBDPrepare : I_PrepareDisplay
    {
        PNBDData PNBDprepare(int date);
        List<string> PNBDreadAllCustomers();
        List<PNBDItemData> PNBDreadAllItems(string custNo);
        int PNBDgetPurchaseQuantity(string custNo, string item, int date, bool isGPI);

    }
}
