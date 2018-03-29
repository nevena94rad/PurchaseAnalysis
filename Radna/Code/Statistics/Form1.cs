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
            int date = DateManipulation.DateTimeToint(dateTimePicker1.Value.AddDays(1));

            os = new OLDStatistics(date);
            ns = new NEWStatistics(parametrsID, date);

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
            DateTime processingDate = this.dateTimePicker1.Value;
            ClearForm();
            List<int> parametersIDs = Parameters.ParametersIDs(processingDate);
            foreach (int param in parametersIDs)
            {
                this.parametersIDs.Items.Add(param);
            }
        }

        private void ClearForm()
        {
            this.parametersIDs.Text = "";
            this.parametersIDs.Items.Clear();
            this.percentageCutOff.Text = "Percentage CutOff:";
            this.countCutOff.Text = "Count CutOff:";
            this.custRecency.Text = "Customer Recency:";
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
            }
            else
            {
                this.button1.Enabled = true;
                this.button2.Enabled = true;
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
    }
}
