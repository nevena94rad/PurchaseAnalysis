using Baza.Prepare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.PrepareCreator
{
    public class CreatePNDBPrepare
    {
        public static readonly List<I_PNBDPrepare> allAvailablePreparers = new List<I_PNBDPrepare> { new SimplePrepare() };
        
        public static List<I_PrepareDisplay> getAll()
        {
            List<I_PrepareDisplay> returnList = new List<I_PrepareDisplay>();

            foreach (var pr in allAvailablePreparers)
                returnList.Add(pr);

            return returnList;
        }
    }
}
