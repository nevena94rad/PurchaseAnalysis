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
                oldPredictions.DataSource = os.occuredPurchases.Where(x => !ns.occuredPurchases.Contains(x));
                newPredictions.DataSource = ns.occuredPurchases.Where(x => !os.occuredPurchases.Contains(x));
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
