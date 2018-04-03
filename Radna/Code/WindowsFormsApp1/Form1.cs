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
using Baza.Enum;
using Baza.Algorithm;
using Baza.Calculators;
using Baza.Prepare;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public System.Timers.Timer timerClock = new System.Timers.Timer();
        public double percent = 10;
        public int recent = 6;
        public int maxCount = 30;
        
        public Form1()
        {
            InitializeComponent();
            dateTimePicker1.Value = Customer.GetLastTransactionDate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Parameters.LoadParameters(DateManipulation.DateTimeToint(dateTimePicker1.Value), (Int32)recency.Value, percentage.Text, count.Text);
            label3.Text = "Start: " + DateTime.Now;
            StopB.Enabled = true;
            StartB.Enabled = false;

            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            PredictionMaker.calculator = new PNBDCalculator(t1_OnProgressUpdate, t2_OnFinishUpdate, backgroundWorker1, new SimplePrepare());
            PredictionMaker.startProccess(DateManipulation.DateTimeToint(dateTimePicker1.Value));
        }

        private void t1_OnProgressUpdate()
        {
            base.Invoke((Action)delegate
            {
                label1.Text = "Done: "+ PredictionMaker.DoneCount + "/" + PredictionMaker.TotalCount;
                label2.Text = "Percentage: " + (((double)PredictionMaker.DoneCount / PredictionMaker.TotalCount) * 100) + "%";
                label4.Text = "Last Write: " + DateTime.Now;
                label5.Text = "Total Writes: " + PredictionMaker.totalWrites;

                var percent = (int)(((double)PredictionMaker.DoneCount / PredictionMaker.TotalCount) * 100);

                progressBar1.Value = percent;
                progressBar1.Refresh();
            });
        }

        private void t2_OnFinishUpdate(string message)
        {
            base.Invoke((Action)delegate
            {
                MessageBox.Show(message);

                StartB.Enabled = true;
                StopB.Enabled = false;
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void percentage_TextChanged(object sender, EventArgs e)
        {
            double value = -1;
            if (!Double.TryParse(percentage.Text, out value) || value <= 0 || value >= 100)
            {
                percentage.Text = percent.ToString();
                MessageBox.Show("Please enter a valid percentage");
            }
            else
            {
                percent = value;
            }
        }

        private void count_TextChanged(object sender, EventArgs e)
        {
            int value = -1;
            if (!Int32.TryParse(count.Text, out value) || value <= 0)
            {
                count.Text = maxCount.ToString();
                MessageBox.Show("Please enter a valid count");
            }
            else
            {
                maxCount = value;
            }
        }

        private void recency_ValueChanged(object sender, EventArgs e)
        {
            if ( recency.Value <= 0)
            {
                recency.Value = recent;
                MessageBox.Show("Please enter a valid number of cutoff months ");
            }
            else
            {
                recent = (int) recency.Value;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
