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
            int parametrsID = Int32.Parse(parametersIDs.SelectedText);
            int date = DateManipulation.DateTimeToint(dateTimePicker1.Value);

            Baza.DTO.Statistics os = new OLDStatistics(date);
            Baza.DTO.Statistics ns = new NEWStatistics(parametrsID, date);

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
            List<int> parametersIDs = Parameters.ParametersIDs(processingDate.AddDays(-1));
            foreach (int param in parametersIDs)
            {
                this.parametersIDs.Items.Add(param);
            }
        }

        
    }
}
