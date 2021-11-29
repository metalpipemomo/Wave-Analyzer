using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace WaveAnalyzer
{
    public partial class FourierForm : Form
    {
        private Complex[] changedWave;
        public FourierForm()
        {
            InitializeComponent();
            chartstyling();
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
        }


        public void plotFourier(double samplerate, string name)
        {
            this.Text = name;
            
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
            changedWave = Fourier.DFT(samples, samples.Length);
        }

        private void FourierChart_Click(object sender, EventArgs e)
        {

        }
    }
}
