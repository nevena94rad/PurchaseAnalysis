using Baza.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.Data
{
    public class ARIMAQuantityData
    {
        public double Value { get; set; }
        public int Date { get; set; }

        public static List<ARIMAQuantityData> TransformQuantityData(List<ARIMAQuantityData> quantity)
        {
            List<ARIMAQuantityData> returnList = new List<ARIMAQuantityData>();
            DateTime current = DateManipulation.intToDateTime(quantity[0].Date);
            DateTime? next = DateManipulation.intToDateTime(quantity[1].Date);
            int daysBetween = ((DateTime)next - current).Days;
            int i = 0;
            int count = quantity.Count();

            while (next != null)
            {
                returnList.Add(new ARIMAQuantityData() { Value = quantity[i].Value / daysBetween, Date = DateManipulation.DateTimeToint(current) });
                current = current.AddDays(1);
                if (current == next)
                {
                    ++i;
                    if (i + 1 < count)
                    {
                        next = DateManipulation.intToDateTime(quantity[i + 1].Date);
                        daysBetween = ((DateTime)next - current).Days;
                    }
                    else
                        next = null;
                }
            }

            return returnList;
        }
    }
}
