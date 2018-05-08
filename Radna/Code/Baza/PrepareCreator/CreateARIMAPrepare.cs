using Baza.Prepare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.PrepareCreator
{
    public static class CreateARIMAPrepare
    {
        public static readonly List<IARIMAPrepare> allAvailablePreparers = new List<IARIMAPrepare> { new SimplePrepare() };

        public static List<IPrepareDisplay> getAll()
        {
            List<IPrepareDisplay> returnList = new List<IPrepareDisplay>();

            foreach (var pr in allAvailablePreparers)
                returnList.Add(pr);

            return returnList;
        }
    }
}
