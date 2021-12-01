using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveAnalyzer
{
    public partial class VerifyForm : Form
    {
        public VerifyForm(Complex[] c)
        {
            InitializeComponent();
            this.Text = "Verification";
            chartStyling();
            plotProof(c);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,
                                                                       Color.FromArgb(0, 0, 0),
                                                                       Color.FromArgb(28, 31, 29),
                                                                       90F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void plotProof(Complex[] c)
        {
            double[] samples = Fourier.inverseDFT(c, c.Length);
            for (int i = 0; i < samples.Length; ++i)
            {
                chart1.Series[0].Points.AddXY(i, samples[i]);
            }
        }

        private void chartStyling()
        {
            var ca = chart1.ChartAreas[0];
            var cs = chart1.Series[0];
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisY.MajorGrid.Enabled = false;
            ca.CursorX.IsUserEnabled = true;
            ca.CursorX.IsUserSelectionEnabled = true;
            ca.AxisX.ScaleView.Zoomable = false;
            ca.AxisY.ScaleView.Zoomable = false;
            ca.AxisX.Minimum = 0;
            ca.AxisX.Maximum = Double.NaN;
            cs.Color = Color.White;
            ca.BackColor = Color.Transparent;
            ca.AxisX.LabelStyle.ForeColor = Color.White;
            ca.AxisY.LabelStyle.ForeColor = Color.White;
        }

    }
}
