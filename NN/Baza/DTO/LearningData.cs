
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public class LearningData
    {
        public List<SinglePointOfData> allData;

        private Object thisLock = new Object();

        private static LearningData _instance;

        public static LearningData Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LearningData();


                return _instance;
            }
        }

        public void addData(SinglePointOfData dataToAdd)
        {
            lock (thisLock)
            {
                allData.Add(dataToAdd);
                Console.WriteLine(allData.Count());
            }
            
        }
        private LearningData()
        {
            allData = new List<SinglePointOfData>();
        }
    }
}
