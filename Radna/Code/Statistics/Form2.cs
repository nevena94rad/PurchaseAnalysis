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
        private bool ignoreSelectedItemsChange;

        public Form2()
        {
            InitializeComponent();

            oldPredictions.DisplayMember = "displayCustomerItem";
            oldPredictions.ValueMember = "displayCustomerItem";
            newPredictions.DisplayMember = "displayCustomerItem";
            newPredictions.ValueMember = "displayCustomerItem";
            intersection.DisplayMember = "displayCustomerItem";
            intersection.ValueMember = "displayCustomerItem";
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
                List<Purchase> intersect = oldCorrect.Where(x => newCorrect.Contains(x,comparer)).OrderBy(x => x.CustNo).ThenBy(x => x.ItemNo).ToList();
                ignoreSelectedItemsChange = true;
                oldPredictions.DataSource = oldMnew;
                newPredictions.DataSource = newMold;
                intersection.DataSource = intersect;
                newCount.Text = "Count:  " + newMold.Count.ToString();
                oldCount.Text = "Count:  " + oldMnew.Count.ToString();
                intersectionCount.Text = "Count:  " + intersect.Count.ToString();
                ignoreSelectedItemsChange = false;

                //draw common compares chart
                chart1.Series["New version"].Points.AddXY("Correct predictions", ns.correctPredictionsPercentage);
                chart1.Series["Old version"].Points.AddXY("Correct predictions", os.correctPredictionsPercentage);
                chart1.Series["New version"].Points.AddXY("False predictions",ns.falsePredictionPercentage);
                chart1.Series["Old version"].Points.AddXY("False predictions", os.falsePredictionPercentage);
                chart1.Series["New version"].Points.AddXY("Coverage",ns.coveragePercentage);
                chart1.Series["Old version"].Points.AddXY("Coverage",os.coveragePercentage);
                
            }
        }

        private void OldSelected(object sender, EventArgs e)
        {
            if (!ignoreSelectedItemsChange)
            {
                Purchase selected = (Purchase)oldPredictions.SelectedItem;
                List<Purchase> purchaseList = Baza.DTO.Statistics.getListOfPurchases(selected.CustNo, selected.ItemNo);
                listOfPurchases.DataSource = purchaseList;
                purchasesCount.Text = "Count:  " + purchaseList.Count.ToString();
            }
        }

        private void newSelected(object sender, EventArgs e)
        {
            if (!ignoreSelectedItemsChange)
            {
                Purchase selected = (Purchase)newPredictions.SelectedItem;
                List<Purchase> purchaseList = Baza.DTO.Statistics.getListOfPurchases(selected.CustNo, selected.ItemNo);
                listOfPurchases.DataSource = purchaseList;
                purchasesCount.Text = "Count:  " + purchaseList.Count.ToString();
            }
        }

        private void intersection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreSelectedItemsChange)
            {
                Purchase selected = (Purchase)intersection.SelectedItem;
                List<Purchase> purchaseList = Baza.DTO.Statistics.getListOfPurchases(selected.CustNo, selected.ItemNo);
                listOfPurchases.DataSource = purchaseList;
                purchasesCount.Text = "Count:  " + purchaseList.Count.ToString();
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
