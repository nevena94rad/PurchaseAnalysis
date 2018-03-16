using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Enum
{
    public class ProcessingStatus
    {
        public enum Status
        {
            SUCCESS = 1,
            ERROR,
            SUSPENDED
        }
    }
}
