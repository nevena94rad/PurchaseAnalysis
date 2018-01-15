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

            c.getAllItems(20171201);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Customer.nextWeekPredictions(20170315);

        }
        
        public void OnTimer(Object source, ElapsedEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var pocetak = DateTime.Now;
            var a =Prediction.makePrediction("FIV108", "10-4784", 20150816,20170425,20171005,6);
            var end = DateTime.Now;
            var diff = end - pocetak;
        }

        
    }
}
