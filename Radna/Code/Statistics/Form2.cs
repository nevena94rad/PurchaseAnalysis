using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Baza.DTO;


namespace Statistics
{
    public partial class Form2 : Form
    {
        public Baza.DTO.Statistics os = null;
        public Baza.DTO.Statistics ns = null;

        public Form2()
        {
            InitializeComponent();

            oldPredictions.DisplayMember = "displayCustomerItem";
            oldPredictions.ValueMember = "displayCustomerItem";
            newPredictions.DisplayMember = "displayCustomerItem";
            newPredictions.DisplayMember = "displayCustomerItem";
            listOfPurchases.DisplayMember = "displayWhole";

        }

        public void setStatistics(Baza.DTO.Statistics os, Baza.DTO.Statistics ns)
        {
            this.os = os;
            this.ns = ns;
        }

        private void onShow(object sender, EventArgs e)
        {
            if(os != null & ns != null)
            {
                PurchaseComperer comparer = new PurchaseComperer();
                List<Purchase> oldCorrect = os.predictedPurchases.Intersect(os.occuredPurchases, comparer).ToList();
                List<Purchase> newCorrect = ns.predictedPurchases.Intersect(ns.occuredPurchases, comparer).ToList();

                List<Purchase> oldMnew = oldCorrect.Where(x => !newCorrect.Contains(x, comparer)).OrderBy(x => x.CustNo).ThenBy(x => x.ItemNo).ToList();
                List<Purchase> newMold = newCorrect.Where(x => !oldCorrect.Contains(x, comparer)).OrderBy(x => x.CustNo).ThenBy(x => x.ItemNo).ToList();
                oldPredictions.DataSource = oldMnew;
                newPredictions.DataSource = newMold;
            }
        }

        private void OldSelected(object sender, EventArgs e)
        {
            Purchase selected = (Purchase) oldPredictions.SelectedItem;
            listOfPurchases.DataSource = Baza.DTO.Statistics.getListOfPurchases(selected.CustNo, selected.ItemNo);
        }

        private void newSelected(object sender, EventArgs e)
        {
            Purchase selected = (Purchase)newPredictions.SelectedItem;
            listOfPurchases.DataSource = Baza.DTO.Statistics.getListOfPurchases(selected.CustNo, selected.ItemNo);
        }
    }
}
