using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WaveAnalyzer {

    public partial class FourierForm : Form {
        private Complex[] changedWave;
        private double[] copiedSamples;
        public bool binselected = false;
        public long timelapsed;
        public int whichfourier;
        public double length;
        public double freqcut;
        public double samplerate;
        public long nonthreadtime;
        private readonly MainForm f;

        /// <summary>
        /// Constructor for Fourier form.
        /// </summary>
        /// <param name="forconvolution">Pointer to form that called it.</param>
        public FourierForm(MainForm forconvolution) {
            InitializeComponent();
            chartstyling();
            f = forconvolution;
        }

        /// <summary>
        /// Overrides paint event. Using it to paint the background with a gradient.
        /// </summary>
        /// <param name="e">Paint event.</param>
        protected override void OnPaintBackground(PaintEventArgs e) {
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,
                                                                       Color.FromArgb(0, 0, 0),
                                                                       Color.FromArgb(28, 31, 29),
                                                                       90F)) {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        /// <summary>
        /// Chart styling.
        /// </summary>
        public void chartstyling() {
            var ca = FourierChart.ChartAreas[0];
            var cs = FourierChart.Series[0];
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisY.MajorGrid.Enabled = false;
            ca.CursorX.IsUserEnabled = true;
            ca.CursorX.SelectionStart = 0;
            ca.AxisX.ScaleView.Zoomable = false;
            ca.AxisY.ScaleView.Zoomable = false;
            button2.Enabled = false;
            button3.Enabled = false;
            numericUpDown1.Enabled = false;
            ca.AxisX.Minimum = 0;
            ca.AxisX.Maximum = Double.NaN;
            cs.Color = Color.White;
            ca.BackColor = Color.Transparent;
            ca.AxisX.LabelStyle.ForeColor = Color.White;
            ca.AxisY.LabelStyle.ForeColor = Color.White;
            ca.AxisX.Minimum = 1;
        }

        /// <summary>
        /// Plots a fourier'd wave.
        /// </summary>
        /// <param name="samplerate">Sample rate of wave.</param>
        /// <param name="name">Name of windowing technique used.</param>
        public void plotFourier(double samplerate, string name) {
            this.samplerate = samplerate;
            this.Text = name;
            label2.Text = (timelapsed).ToString() + " ms";
            label3.Text = (nonthreadtime).ToString() + " ms";
            Complex[] f = changedWave;
            double[] freqs = Fourier.getFrequency(f, samplerate);
            length = Fourier.getLargestFreq(freqs);
            double[] amps = Fourier.getAmplitudes(f);
            Trace.WriteLine(f.Length + " " + freqs.Length + " " + amps.Length);
            FourierChart.Series[0].Points.Clear();
            for (int i = 0; i < f.Length; ++i) {
                FourierChart.Series[0].Points.AddXY(freqs[i], amps[i]);
            }
        }

        /// <summary>
        /// Calculates the fourier of passed in samples.
        /// </summary>
        /// <param name="samples">Sample array.</param>
        public void doFourier(double[] samples) {
            copiedSamples = new double[samples.Length];
            Array.Copy(samples, copiedSamples, copiedSamples.Length);
            if (whichfourier == 1) {
                changedWave = Fourier.DFT(samples, samples.Length);
            }
            else {
                changedWave = Fourier.nonThreadedDFT(samples, samples.Length);
            }
        }

        /// <summary>
        /// Creates a window to verify that fourier was done correctly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {
            VerifyForm f = new VerifyForm(changedWave);
            f.Show();
        }

        /// <summary>
        /// Creates a low-pass filter and convolutes original wave.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e) {
            if (numericUpDown1.Value < 32) {
                numericUpDown1.Value = 32;
            }
            double[] s = Fourier.lowPassFilter((int)numericUpDown1.Value, freqcut, samplerate);
            Complex[] idftfw = Fourier.convertFilter(s);
            double[] fw = Fourier.inverseDFT(idftfw, idftfw.Length);
            f.fileSamples = Fourier.convolve(f.fileSamples, fw);
            copiedSamples = Fourier.convolve(copiedSamples, fw);
            f.plotFreqWaveChart(f.fileSamples);
            f.SetBufferInOtherWindow();
            doFourier(copiedSamples);
            plotFourier(samplerate, this.Text);
        }
        
        /// <summary>
        /// Creates a high-pass filter and convolutes original wave.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e) {
            if (numericUpDown1.Value < 32) {
                numericUpDown1.Value = 32;
            }
            double[] s = Fourier.highPassFilter((int)numericUpDown1.Value, freqcut, samplerate);
            Complex[] idftfw = Fourier.convertFilter(s);
            double[] fw = Fourier.inverseDFT(idftfw, idftfw.Length);
            f.fileSamples = Fourier.convolve(f.fileSamples, fw);
            f.plotFreqWaveChart(f.fileSamples);
            f.SetBufferInOtherWindow();
            doFourier(copiedSamples);
            plotFourier(samplerate, this.Text);
        }

        /// <summary>
        /// Bin selection and controlling cursor position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FourierChart_Click(object sender, EventArgs e) {
            var ca = FourierChart.ChartAreas[0];
            if (ca.CursorX.Position > length / 2) {
                ca.CursorX.Position = length / 2;
            }
            ca.CursorX.SelectionEnd = ca.CursorX.Position;
            freqcut = ca.CursorX.Position;
            button2.Enabled = true;
            button3.Enabled = true;
            numericUpDown1.Enabled = true;
        }

    }
}