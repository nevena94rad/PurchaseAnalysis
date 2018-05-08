using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.GPISelector
{
    public interface I_GPISelector
    {
        string Select(List<string> items, string customer, string GPInumber);
    }
}
