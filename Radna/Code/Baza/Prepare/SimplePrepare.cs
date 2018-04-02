using Baza.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baza.DTO;

namespace Baza.Prepare
{
    public class SimplePrepare
    {
        /// <summary>
        /// Ucitava i obradjuje podatke
        /// </summary>
        /// <returns></returns>
        public static PNBDData PNBDprepare(int date)
        {
            PNBDData returnData = new PNBDData();

            returnData.AllCustomers = Customer.GetAllCustomers(date);

            return returnData;
        }
    }
}
