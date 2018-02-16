
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public class ConsumptionData
    {
        public List<ItemConsumption> allData;

        private Object thisLock = new Object();

        private static ConsumptionData _instance;

        public static ConsumptionData Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConsumptionData();


                return _instance;
            }
        }

        public void addData(ItemConsumption dataToAdd)
        {
            
                allData.Add(dataToAdd);
            
        }
        public ItemConsumption readData(string itemNo)
        {
            
                var returnItem = (from item in allData
                                  where item.itemNo == itemNo
                                  select item).SingleOrDefault();

                return returnItem;
            
        }
        private ConsumptionData()
        {
            allData = new List<ItemConsumption>();
        }
    }
}
