using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveAnalyzer
{
    public partial class FreqCut : Form
    {
        public FreqCut()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            Form1.freqinput = int.Parse(textBox1.Text);
            DialogResult = DialogResult.OK;
        }
    }
}
