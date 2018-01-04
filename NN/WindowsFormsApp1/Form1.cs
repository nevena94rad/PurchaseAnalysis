using Baza.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
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
                custNo = "SUT101"
            };

            var t1 = DateTime.Now;
            c.getAllItems();
            var t2 = DateTime.Now;
            var diff = t2 - t1;

            MessageBox.Show("" + c.itemNos.Count);
            MessageBox.Show("" + diff.TotalMilliseconds);
        }
    }
}
