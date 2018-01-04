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
            // testirano, radi sporo, 5 sec po customeru
            // naci alternativu (nova tabela?)

            var db = new DataClasses1DataContext();

            var allItems = from customer in db.PurchaseHistories
                           where customer.CustNo == custNo
                           group customer by customer.ItemNo into groupedCustomer
                           where groupedCustomer.Count() > 26
                           select groupedCustomer.Key;


            itemNos = allItems.ToList();
                
                            
            // ucitati sve iteme koje je cust narucio vise od 2 puta (3+) 
           
        }
        public List<Prediction> makeAllPredictions()
        {
            List<Prediction> returnList = new List<Prediction>();

            var db = new DataClasses1DataContext();
            
            foreach (var item in itemNos)
            {
                var allDates = from purchase in db.PurchaseHistories
                               where purchase.CustNo == custNo && purchase.ItemNo == item
                               orderby purchase.InvDate
                               select purchase.InvDate;

                List<DateTime> listOfDates = allDates.ToList().ConvertAll<DateTime>(delegate (int i)
                    {
                        return DateTime.ParseExact(i.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                    });

                var start = listOfDates.First();
<<<<<<< HEAD

                listOfDates.RemoveRange(0, 2);

                foreach(var date in listOfDates)

=======
<<<<<<< HEAD
                listOfDates.RemoveRange(0, 2);

                foreach(var date in listOfDates)
=======
>>>>>>> f39f3dae9dd6bca1e4bb37ec5b3364152db1b322
                listOfDates.RemoveAt(0);
                int numOfDates = listOfDates.Count();

                for (int i= 0; i< numOfDates-1 ;++i)
>>>>>>> a1c740ecab208c55b9c8bf47e7abfd97c93c5a5e
                {
                    returnList.Add(Prediction.makePrediction(custNo, item, start, date));
                }
 
            }
            // za svaki item se prave predikcije
            // ako item ima n kupovina od strane customera (n>=3)
            // predikcije se prave na osnovu k datuma gde je ( 2<=k<n)
            // za svaku predikciju se pamti za koj period je izvrsena, kad je predvidjena i kad se desila sl kupovina

            return returnList;
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
            throw new Exception();
        }
    }
}
