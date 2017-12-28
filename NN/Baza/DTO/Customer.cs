using System;
using System.Collections.Generic;
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
            throw new Exception();
        }
        public List<Prediction> makeAllPredictions()
        {
            // za svaki item se prave predikcije
            // ako item ima n kupovina od strane customera (n>=3)
            // predikcije se prave na osnovu k datuma gde je ( 2<=k<n)
            // za svaku predikciju se pamti za koj period je izvrsena, kad je predvidjena i kad se desila sl kupovina
            throw new Exception();
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
