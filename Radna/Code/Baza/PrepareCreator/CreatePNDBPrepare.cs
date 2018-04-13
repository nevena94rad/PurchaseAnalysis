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
        public static readonly List<PNBDPrepare> allAvailablePreparers = new List<PNBDPrepare> { new SimplePrepare() };
        
        public static List<PrepareDispley> getAll()
        {
            List<PrepareDispley> returnList = new List<PrepareDispley>();

            foreach (var pr in allAvailablePreparers)
                returnList.Add(pr);

            return returnList;
        }
    }
}
