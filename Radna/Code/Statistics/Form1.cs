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
    public partial class Form1 : Form
    {
        public Form2 form2 = new Form2();
        public Baza.DTO.Statistics os = null;
        public Baza.DTO.Statistics ns = null;

        public Form1()
        {
            InitializeComponent();

            cutOffPercentage.Items.Add(0.0);
            cutOffPercentage.Items.Add(0.01);
            cutOffPercentage.Items.Add(0.1);
            cutOffPercentage.Items.Add(1.0);
            cutOffPercentage.Items.Add(5.0);
            cutOffPercentage.Items.Add(10.0);
            cutOffPercentage.Items.Add(25.0);
            cutOffPercentage.Items.Add(50.0);
            cutOffPercentage.Items.Add(70.0);
            cutOffPercentage.Items.Add(90.0);

            cutOffPercentage.SelectedItem = 0.01;

            this.availableDates.DataSource = Parameters.GetProcessingDates();
            //DateTime processingDate = ((DateTime)availableDates.SelectedItem);
            //List<int> parametersIDs = Parameters.ParametersIDs(processingDate);
            //foreach (int param in parametersIDs)
            //{
            //    this.parametersIDs.Items.Add(param);
            //}
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;

                int parametrsID = (int)(parametersIDs.SelectedItem);
                int date = DateManipulation.DateTimeToint(((DateTime)availableDates.SelectedItem).AddDays(1));

                os = new OLDStatistics(date, FirstAndSecondPurchase.Checked);
                ns = new NEWStatistics(parametrsID, date, FirstAndSecondPurchase.Checked, (double)cutOffPercentage.SelectedItem);

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

                this.UseWaitCursor = false;
            }
            finally
            {
                this.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
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
            bool useGpi = false;
            parameters = Parameters.GetParameters(parametersID, out status, out useGpi);
            
            this.custRecency.Text = "Customer Recency:  " + parameters["customerRecency"];
            this.percentageCutOff.Text = "Percentage CutOff:  " + parameters["predictionPercentageCutOff"];
            this.countCutOff.Text = "Count CutOff:  " + parameters["predictionCountCutOff"];
            if(parameters["calculator"] != null && parameters["calculator"] != "")
                this.model.Text = "Model:   " + parameters["calculator"];
            else
                this.model.Text = "Model:   Not specified";
            if(parameters["preparer"] != null && parameters["preparer"] != "")
                this.preparer.Text = "Preparer:   " + parameters["preparer"];
            else
                this.preparer.Text = "Preparer:   Not specified";

            //gpi used
            if (useGpi)
            {
                this.gpi.Text = "GPI used: YES";
                this.gpiDigits.Text = "GPI digits: " + parameters["gpiDigits"];
                this.gpiResult.Text = "GPI result: " + parameters["gpiResult"];
            }
            //gpi not used
            else
            {
                this.gpi.Text = "GPI used: NO";
                this.gpiDigits.Text = "";
                this.gpiResult.Text = "";
            }

            this.status.Text = "Status:  " + status;

            if (status != Baza.Enum.ProcessingStatus.Status.SUCCESS.ToString())
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
