using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace WaveAnalyzer
{
    public partial class FourierForm : Form
    {
        private Complex[] changedWave;
        public bool binselected = false;
        public long timelapsed;
        public int whichfourier;
        public FourierForm()
        {
            InitializeComponent();
            chartstyling();
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

        public void chartstyling()
        {
            var ca = FourierChart.ChartAreas[0];
            var cs = FourierChart.Series[0];
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


        public void plotFourier(double samplerate, string name)
        {
            this.Text = name;
            label2.Text = (timelapsed).ToString() + " ms";
            Complex[] f = changedWave;
            double[] freqs = Fourier.getFrequency(f, samplerate);
            double[] amps = Fourier.getAmplitudes(f);
            Trace.WriteLine(f.Length + " " + freqs.Length + " " + amps.Length);
            for (int i = 0; i < f.Length; ++i)
            {
                FourierChart.Series[0].Points.AddXY(freqs[i], amps[i]);
            }
        }

        public void doFourier(double[] samples)
        {
            if (whichfourier == 1) {
                changedWave = Fourier.DFT(samples, samples.Length);
            } else {
                changedWave = Fourier.DFTpt2(samples, samples.Length);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VerifyForm f = new VerifyForm(changedWave);
            f.Show();
        }
    }
}
