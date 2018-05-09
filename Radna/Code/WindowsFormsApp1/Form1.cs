﻿using Baza.DTO;
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
        Abs_Calculator selectedCalculator;
        I_PrepareDisplay selectedPreparer;
        public bool exitEnabled = true;
        public bool exit = false;
        
        public Form1()
        {
            InitializeComponent();
            DateTime lastTransactionDate = Customer.GetLastTransactionDate();
            dateTimePicker1.Value = lastTransactionDate.AddDays(-7);

            regularItems.Checked = true;

            selectGPIdigits.Maximum = GPI.getMaxNumOfDigits();
            selectGPIdigits.Value = selectGPIdigits.Maximum;

            calculator.DisplayMember = "displayText";
            calculator.DataSource = PredictionMaker.getAllCalculators(t1_OnProgressUpdate, t2_OnFinishUpdate, backgroundWorker1);

            setPreparer();
        }

        public void setPreparer()
        {
            preparer.DataSource = null;
            preparer.Items.Clear();
            preparer.DisplayMember = "displayName";
            Abs_Calculator selected = (Abs_Calculator) calculator.SelectedItem;
            preparer.DataSource = selected.allAvailablePreparers;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Parameters.LoadParameters(DateManipulation.DateTimeToint(dateTimePicker1.Value), (Int32)recency.Value, percentage.Text, count.Text, 
                                        ((Abs_Calculator)calculator.SelectedValue).displayText, ((I_PrepareDisplay)preparer.SelectedValue).displayName, 
                                        (int)selectGPIdigits.Value, useGPI.Checked);
            label3.Text = "Start: " + DateTime.Now;
            StopB.Enabled = true;
            StartB.Enabled = false;
            exitEnabled = false;

            selectedCalculator = (Abs_Calculator)calculator.SelectedItem;
            selectedPreparer = (I_PrepareDisplay)preparer.SelectedItem;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            PredictionMaker.calculator = selectedCalculator;
            selectedCalculator.setPreparer(selectedPreparer);
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
                exitEnabled = true;

                if (exit)
                    base.Close();
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

        private void calculator_SelectedIndexChanged(object sender, EventArgs e)
        {
            setPreparer();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (exitEnabled == false)
            {
                backgroundWorker1.CancelAsync();
                exit = true;
                e.Cancel = true;
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(regularItems.Checked == true)
            {
                this.gpiDigits.ForeColor = System.Drawing.SystemColors.GrayText;
                this.selectGPIresult.ForeColor = System.Drawing.SystemColors.GrayText;
                this.selectGPIdigits.Enabled = false;
                this.GPIresult.Enabled = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (useGPI.Checked == true)
            {
                this.gpiDigits.ForeColor = System.Drawing.SystemColors.ControlText;
                this.selectGPIresult.ForeColor = System.Drawing.SystemColors.ControlText;
                this.selectGPIdigits.Enabled = true;
                this.GPIresult.Enabled = true;
            }
        }

        private void selectGPIdigits_ValueChanged(object sender, EventArgs e)
        {
        }

        
    }
}
