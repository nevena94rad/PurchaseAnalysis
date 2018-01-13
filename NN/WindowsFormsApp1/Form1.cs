using Baza.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public System.Timers.Timer timerClock = new System.Timers.Timer();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Customer c = new Customer
            {
                custNo = "PRO275"
            };

            var t1 = DateTime.Now;
            c.getAllItems();
            var t2 = DateTime.Now;
            var diff = t2 - t1;
            var dates = c.makeAllPredictions();


            MessageBox.Show("" + c.itemNos.Count);
            MessageBox.Show("" + diff.TotalMilliseconds);
            foreach (var date in dates)
                MessageBox.Show(date.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timerClock.Elapsed += new ElapsedEventHandler(OnTimer);
            timerClock.Interval = 10000;
            timerClock.Enabled = true;
            Prediction.init();
            Thread t = new Thread(Customer.getAllCustomerData);
            t.Start();
            
        }
        
        public void OnTimer(Object source, ElapsedEventArgs e)
        {
            MessageBox.Show(Prediction.count.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var pocetak = DateTime.Now;
            Prediction.makePredictionAlternativeWay("FIV108", "10-4784", 20150816,20170425,20171005,2);
            var end = DateTime.Now;
            var diff = end - pocetak;
        }

        
    }
}
