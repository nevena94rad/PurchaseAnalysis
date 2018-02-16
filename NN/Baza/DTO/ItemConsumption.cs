using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public class ItemConsumption
    {
        public string itemNo;
        public int startDate;
        public List<double> listOfValues;

        public static void readAllItemData(int nextPurchase)
        {
            var db = new DataClasses1DataContext();

            var allItems = (from item in db.ItemConsumptions
                            where item.Date < nextPurchase
                            select item.ItemNo).Distinct();

            foreach(var item in allItems)
            {
                var itemConsumption = (from purchases in db.ItemConsumptions
                                       where purchases.ItemNo == item && purchases.Date < nextPurchase
                                       select new DailyValue { Value = purchases.Consumption, Date = purchases.Date }).OrderBy(x => x.Date);

                List<double> Consumption = new List<double>();
                foreach (var itemCons in itemConsumption)
                {
                    Consumption.Add(itemCons.Value);
                }

                var first = (from itemPurchase in itemConsumption
                             select itemPurchase.Date).Min();

                ConsumptionData.Instance.addData(new ItemConsumption() { itemNo = item, listOfValues = Consumption, startDate = first });
            }
        }
    }
}
