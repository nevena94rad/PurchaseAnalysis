﻿using System;
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
    public partial class Form1 : Form
    {
        public Form2 form2 = new Form2();
        public Baza.DTO.Statistics os = null;
        public Baza.DTO.Statistics ns = null;

        public Form1()
        {
            InitializeComponent();

            cutOffPercentage.Items.Add(0);
            cutOffPercentage.Items.Add(0.01);
            cutOffPercentage.Items.Add(0.1);
            cutOffPercentage.Items.Add(1);
            cutOffPercentage.Items.Add(5);
            cutOffPercentage.Items.Add(10);
            cutOffPercentage.Items.Add(25);
            cutOffPercentage.Items.Add(50);

            cutOffPercentage.SelectedItem = 0.01;

            this.availableDates.DataSource = Parameters.GetProcessingDates();
            //DateTime processingDate = ((DateTime)availableDates.SelectedItem);
            //List<int> parametersIDs = Parameters.ParametersIDs(processingDate);
            //foreach (int param in parametersIDs)
            //{
            //    this.parametersIDs.Items.Add(param);
            //}
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int parametrsID = (int)(parametersIDs.SelectedItem);
            int date = DateManipulation.DateTimeToint(((DateTime)availableDates.SelectedItem).AddDays(1));

            os = new OLDStatistics(date, FirstAndSecondPurchase.Checked);
            ns = new NEWStatistics(parametrsID, date, FirstAndSecondPurchase.Checked, (double) cutOffPercentage.SelectedItem);

            OLD_NoP.Text = "Number of predictions: " + os.predictionCount;
            OLD_CPtp.Text = "Total number: " + os.correctPredictionsCount;
            OLD_CPp.Text = "Percentage: " + os.correctPredictionsPercentage;
            OLD_FPtp.Text = "Total number: " + os.falsePredictionCount;
            OLD_FPp.Text = "Percentage: " + os.falsePredictionPercentage;
            OLD_Cp.Text = "Percentage: " + os.coveragePercentage;

            NEW_NoP.Text = "Number of predictions: " + ns.predictionCount;
            NEW_CPtp.Text = "Total number: " + ns.correctPredictionsCount;
            NEW_CPp.Text = "Percentage: " + ns.correctPredictionsPercentage;
            NEW_FPtp.Text = "Total number: " + ns.falsePredictionCount;
            NEW_FPp.Text = "Percentage: " + ns.falsePredictionPercentage;
            NEW_Cp.Text = "Percentage: " + ns.coveragePercentage;

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void ClearForm()
        {
            this.parametersIDs.Text = "";
            this.parametersIDs.Items.Clear();
            this.percentageCutOff.Text = "Percentage CutOff:";
            this.countCutOff.Text = "Count CutOff:";
            this.custRecency.Text = "Customer Recency:";
            this.status.Text = "Status:";
            this.status.ForeColor = Color.Black;
            this.button1.Enabled = false;
            this.button2.Enabled = false;
        }

        private void parametersIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            int parametersID = (int)this.parametersIDs.SelectedItem;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string status = "";
            parameters = Parameters.GetParameters(parametersID, out status);

            this.custRecency.Text = "Customer Recency:  " + parameters["customerRecency"];
            this.percentageCutOff.Text = "Percentage CutOff:  " + parameters["predictionPercentageCutOff"];
            this.countCutOff.Text = "Count CutOff:  " + parameters["predictionCountCutOff"];
            this.status.Text = "Status:  " + status;

            if (status == Baza.Enum.ProcessingStatus.Status.ERROR.ToString() || status == Baza.Enum.ProcessingStatus.Status.SUSPENDED.ToString())
            {
                this.button1.Enabled = false;
                this.button2.Enabled = false;
                this.status.ForeColor = Color.Red;
            }
            else
            {
                this.button1.Enabled = true;
                this.button2.Enabled = true;
                this.status.ForeColor = Color.Green;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (os != null && ns != null)
            {
                form2.setStatistics(os, ns);
                form2.ShowDialog();
            }
        }

        private void avableDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime processingDate = ((DateTime)availableDates.SelectedItem);
            ClearForm();
            List<int> parametersIDs = Parameters.ParametersIDs(processingDate);
            foreach (int param in parametersIDs)
            {
                this.parametersIDs.Items.Add(param);
            }
        }

        private void status_Click(object sender, EventArgs e)
        {

        }
    }
}
