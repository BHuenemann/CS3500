using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ComputeTip_Click(object sender, EventArgs e)
        {
            Double.TryParse(BillTextBox.Text, out double bill);
            Double.TryParse(TipBox.Text, out double tip);
            TipTextBox.Text = (.01 * tip * bill).ToString();
            TotalBox.Text = ((.01 * tip * bill) + bill).ToString();
        }

        private void BillTextBox_TextChanged(object sender, EventArgs e)
        {
            ComputeTip.Enabled = Double.TryParse(BillTextBox.Text, out _) && Double.TryParse(TipBox.Text, out _);
            if(ComputeTip.Enabled)
                ComputeTip_Click(sender, e);
        }

        private void TipBox_TextChanged(object sender, EventArgs e)
        {
            ComputeTip.Enabled = Double.TryParse(BillTextBox.Text, out _) && Double.TryParse(TipBox.Text, out _);
            if(ComputeTip.Enabled)
                ComputeTip_Click(sender, e);
        }
    }
}
