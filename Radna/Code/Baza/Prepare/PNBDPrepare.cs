using Baza.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Prepare
{
    public interface PNBDPrepare
    {
        PNBDData PNBDprepare(int date);
        List<string> PNBDreadAllCustomers();
        List<PNBDItemData> PNBDreadAllItems(string custNo);
        
    }
}
