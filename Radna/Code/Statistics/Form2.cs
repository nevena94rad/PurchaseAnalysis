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
    public partial class Form2 : Form
    {
        public Baza.DTO.Statistics os = null;
        public Baza.DTO.Statistics ns = null;

        public Form2()
        {
            InitializeComponent();
        }
        
        public void setStatistics(Baza.DTO.Statistics os, Baza.DTO.Statistics ns)
        {
            this.os = os;
            this.ns = ns;
        }


        
    }
}
