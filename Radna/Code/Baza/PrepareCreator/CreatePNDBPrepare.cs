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
        public static readonly List<IPNBDPrepare> allAvailablePreparers = new List<IPNBDPrepare> { new SimplePrepare() };
        
        public static List<IPrepareDisplay> getAll()
        {
            List<IPrepareDisplay> returnList = new List<IPrepareDisplay>();

            foreach (var pr in allAvailablePreparers)
                returnList.Add(pr);

            return returnList;
        }
    }
}
