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
            label3.Text = "Start: " + DateTime.Now;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //ItemConsumption.readAllItemData(20170517);
            //var a = Prediction.makePrediction("GUA119", "02-1500", 20161117, 20161212, 20170512, 1);
            Customer c = new Customer() { custNo = "GUA119" };
            for(int i=0; i< 100; ++i)
                c.getAllItems(20170512);
            //Customer.nextWeekPredictions(20170606, t1_OnProgressUpdate);
        }
        private void t1_OnProgressUpdate()
        {
            base.Invoke((Action)delegate
            {
                label1.Text = "Done: "+Customer.Dcount + "/" + Customer.Tcount;
                label2.Text = "Percentage: " + (((double)Customer.Dcount / Customer.Tcount) * 100) + "%";
                label4.Text = "Last Write: " + DateTime.Now;
                label5.Text = "Total Writes: " + Customer.totalWrites;
            });
        }
    }
}
