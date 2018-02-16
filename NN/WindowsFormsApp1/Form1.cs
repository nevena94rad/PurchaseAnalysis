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
            Customer.nextWeekPredictions(20170606, t1_OnProgressUpdate);
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
