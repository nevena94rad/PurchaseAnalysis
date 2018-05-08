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
        public static readonly List<I_ARIMAPrepare> allAvailablePreparers = new List<I_ARIMAPrepare> { new SimplePrepare() };

        public static List<I_PrepareDisplay> getAll()
        {
            List<I_PrepareDisplay> returnList = new List<I_PrepareDisplay>();

            foreach (var pr in allAvailablePreparers)
                returnList.Add(pr);

            return returnList;
        }
    }
}
