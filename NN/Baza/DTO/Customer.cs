using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baza.DTO
{
    public class Customer
    {
        public List<string> itemNos;
        public string custNo;

        public void getAllItems()
        {
            // ucitati sve iteme koje je cust narucio vise od 2 puta (3+)
            // testirano, radi sporo, 5 sec po customeru
            // naci alternativu (nova tabela?)


            var db = new DataClasses1DataContext();

            var allItems = from customer in db.PurchaseHistories
                           where customer.CustNo == custNo && customer.InvDate<20170600
                           group customer by customer.ItemNo into groupedCustomer
                           where groupedCustomer.Count() > 4
                           select groupedCustomer.Key;


            itemNos = allItems.ToList();
            
        }
        public List<Prediction> makeAllPredictions()
        {
            // za svaki item se prave predikcije
            // ako item ima n kupovina od strane customera (n>=3)
            // predikcije se prave na osnovu k datuma gde je ( 2<=k<n)
            // za svaku predikciju se pamti za koj period je izvrsena, kad je predvidjena i kad se desila sl kupovina

            List<Prediction> returnList = new List<Prediction>();

            var db = new DataClasses1DataContext();
            
            foreach (var item in itemNos)
            {
                var allDates = (from purchase in db.PurchaseHistories
                               where purchase.CustNo == custNo && purchase.ItemNo == item
                               && purchase.InvDate<20170600
                               orderby purchase.InvDate
                               select purchase.InvDate).Distinct();

                List<int> listOfDates = allDates.ToList();

                var start = listOfDates.First();
                
                listOfDates.RemoveAt(0);
                int numOfDates = listOfDates.Count();

                for (int i= 0; i< numOfDates-1 ;++i)
                {
                    int quanaty = getPurchaseQuantity(item, listOfDates[i]);

                    var prediction = Prediction.makePredictionAlternativeWay(custNo, item, start, listOfDates[i], listOfDates[i + 1], quanaty);

                    if (prediction.predictedConsumption>=0)
                        returnList.Add(prediction);
                }
 
            }
            

            return returnList;
        }
        private int getPurchaseQuantity(string item, int date)
        {
            var db = new DataClasses1DataContext();

            var quantityQuery = (from purchase in db.PurchaseHistories
                                 where purchase.CustNo == custNo &&
                                         purchase.ItemNo == item &&
                                         purchase.InvDate == date
                                 select purchase.InvQty).Sum();


            return quantityQuery;
        }
        public void addToLearningData()
        {
            // pozove se makeAllPredictions
            // na osnovu tih predikcija treba napraviti vise SinglePointOfData
            // svaki SPD se vezuje za jednu Predikciju
            // ako je predikcija bila na osnovu perioda a-b
            // u SPD se pravi statistika svih Predikcija koje su se desile do trenutka b
            // treba ih svrstati u grupe do 2% do 5% do 10% i vise od 10% kako za taj item tako i za sve ostale
            // na kraju se svi dodaju u LearningData

            getAllItems();
            List<Prediction> allCustomerPredictions = makeAllPredictions();
            allCustomerPredictions.OrderBy(x => x.to);
            
            int predictionCount = allCustomerPredictions.Count();

            for (int i = 0; i < predictionCount; ++i)
            {
                SinglePointOfData newData = new SinglePointOfData();

                for (int j = 0; j < i; ++j)
                {
                    double previusError = allCustomerPredictions[j].getError();

                    if (allCustomerPredictions[j].itemNo == allCustomerPredictions[i].itemNo)
                        newData.addToItem(previusError);
                    else
                        newData.addToTotall(previusError);
                }

                newData.normalize();
                double currentError = allCustomerPredictions[i].getError();
                newData.addCategory(currentError);

                LearningData.Instance.addData(newData);
            }
            
        }
        public static void getAllCustomerData()
        {
            List<Customer> returnList = new List<Customer>();

            var db = new DataClasses1DataContext();

            var allCustomers = (from customer in db.PurchaseHistories
                                select customer.CustNo).Distinct();

            int custCount = allCustomers.Count();
            var listCust = allCustomers.ToList();
            Parallel.For(0, custCount,new ParallelOptions { MaxDegreeOfParallelism = 50}, i =>
            {
                Customer newCustomer = new Customer()
                {
                    custNo = listCust[i]
                };

                newCustomer.addToLearningData();
            });
            
        }
    }
}
