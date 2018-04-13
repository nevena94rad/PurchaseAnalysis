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
        public static readonly List<ARIMAPrepare> allAvailablePreparers = new List<ARIMAPrepare> { new SimplePrepare() };

        public static List<PrepareDispley> getAll()
        {
            List<PrepareDispley> returnList = new List<PrepareDispley>();

            foreach (var pr in allAvailablePreparers)
                returnList.Add(pr);

            return returnList;
        }
    }
}
