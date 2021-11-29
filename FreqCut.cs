using System;
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
