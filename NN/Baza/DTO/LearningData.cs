
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

        private static LearningData _instance;

        public LearningData Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LearningData();


                return _instance;
            }
        }

        private LearningData()
        {
            allData = new List<SinglePointOfData>();
        }
    }
}
