using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Win32;
using System.Runtime;

namespace WaveAnalyzer
{
    public partial class Form1 : Form
    {
        //need to marshal or something to get hinstance and pstr
        [DllImport("Record.dll", CallingConvention = CallingConvention.Winapi)] public static extern int start();
        private string filePath;
        private double[] globalFreq;
        //private double[] globalAmp;
        private double[] copy;
        private double xstart;
        private double xend;
        private Color linecolor = Color.FromArgb(255, 105, 180);
        private WavReader globalWavHdr = new WavReader();
        public Form1()
        {
            InitializeComponent();
            chartStyling();
            buttonStyling();
            double[] s = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14};
            double[] fw = {1, 0.2, 0.2, 0, 1, 0.1, 0.1, 1};
            double[] samples = Fourier.convolve(s, fw);
            Trace.WriteLine("Convolution Test:");
            Fourier.printSamplesTrace(samples);
            Trace.WriteLine("Filter Test:");
            double srate = 1000;
            double fcut = 300;
            int size = 16;
            double[] myfilter = Fourier.lowPassFilter(size, fcut, srate);
            Fourier.printSamplesTrace(myfilter);
            myfilter = Fourier.highPassFilter(size, fcut, srate);
            Fourier.printSamplesTrace(myfilter);
            double[] shannonentropytest = {5, 10, 15, 10, 8, 10, 15, 5, 8, 3, 8, 5, 8, 10, 10, 15, 10, 10, 8, 5, 5};
            Trace.WriteLine("Shannon Entropy Test:");
            Trace.WriteLine(Fourier.entropy(shannonentropytest));
            Fourier.printSamplesTrace(Fourier.uniquearr(shannonentropytest));
            Fourier.printSamplesTrace(Fourier.inverseDFT(Fourier.convertFilter(myfilter), myfilter.Length));
        }

        private void chartStyling()
        {
            var ca = chart1.ChartAreas[0];
            var cs = chart1.Series["Original"];
            ca.AxisX.Minimum = 0;
            ca.AxisX.Maximum = Double.NaN;
            ca.AxisX.ScrollBar.Enabled = true;
            ca.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            ca.AxisX.ScaleView.Zoomable = false;
            ca.CursorX.IsUserEnabled = true;
            ca.CursorX.IsUserSelectionEnabled = true;
            cs.Color = linecolor;
            ca.CursorX.SelectionColor = linecolor;
            ca.BackColor = Color.SlateGray;
            ca.AxisX.ScrollBar.BackColor = Color.White;
        }

        private void buttonStyling()
        {
            Cut.Enabled = false;
            Copy.Enabled = false;
            Paste.Enabled = false;
            Clear.Enabled = false;
            Save.Enabled = false;
        }

        public double[] readingWave(String fileName)
        {
            byte[] byteArray;
            BinaryReader reader = new BinaryReader(System.IO.File.OpenRead(fileName));
            globalWavHdr.Empty();
            globalWavHdr.ChunkID = reader.ReadInt32();
            globalWavHdr.ChunkSize = reader.ReadInt32();
            globalWavHdr.Format = reader.ReadInt32();
            globalWavHdr.SubChunk1ID = reader.ReadInt32();
            globalWavHdr.SubChunk1Size = reader.ReadInt32();
            globalWavHdr.AudioFormat = reader.ReadUInt16();
            globalWavHdr.NumChannels = reader.ReadUInt16();
            globalWavHdr.SampleRate = reader.ReadUInt32();
            globalWavHdr.ByteRate = reader.ReadUInt32();
            globalWavHdr.BlockAlign = reader.ReadUInt16();
            globalWavHdr.BitsPerSample = reader.ReadUInt16();
            globalWavHdr.SubChunk2ID = reader.ReadInt32();
            globalWavHdr.SubChunk2Size = reader.ReadInt32();
            byteArray = reader.ReadBytes((int)globalWavHdr.SubChunk2Size);
            short[] shortArray = new short[globalWavHdr.SubChunk2Size / globalWavHdr.BlockAlign];
            double[] outputArray;
            for (int i = 0; i < globalWavHdr.SubChunk2Size / globalWavHdr.BlockAlign; i++)
            {
                shortArray[i] = BitConverter.ToInt16(byteArray, i * globalWavHdr.BlockAlign);
            }
            outputArray = shortArray.Select(x => (double)(x)).ToArray();
            reader.Close();
            return outputArray;
        }

        public void OpenFile(string fileName)
        {
            filePath = fileName;
            Text = fileName;
            globalFreq = readingWave(filePath);
            plotFreqWaveChart(globalFreq);
        }

        public void plotFreqWaveChart(double[] array)
        {
            chart1.Series["Original"].Points.Clear();
            for (int i = 0; i < array.Length; i++)
            {
                chart1.Series["Original"].Points.AddXY(i, array[i]);
            }
            chart1.ChartAreas[0].AxisX.ScaleView.Size = array.Length / 100;
            Clear.Enabled = true;
            Save.Enabled = true;
            Cut.Enabled = true;
            Copy.Enabled = true;
        }

        private void File_Click(object sender, EventArgs e)
        {
            //Basically the menu where you select files
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //add filter
            openFileDialog.Filter = "WAV File (*.wav)|*.wav|All files (*.*)|*.*";
            //check if file open
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenFile(openFileDialog.FileName);
            }
            
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            xstart = chart1.ChartAreas[0].CursorX.SelectionStart;
            xend = chart1.ChartAreas[0].CursorX.SelectionEnd;
            Trace.WriteLine(xstart + "\n" + xend);
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            chart1.Series["Original"].Points.Clear();
            buttonStyling();
        }

        private void Save_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            start();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            copy = new double[(int) xend - (int) xstart + 1];
            int nums = 0;
            for (int i = (int) xstart; i <= (int) xend; i++)
            {
                copy[nums] = globalFreq[i];
                nums++;
            }
            Paste.Enabled = true;
            Fourier.printSamplesTrace(copy);
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            double[] newfreq = new double[globalFreq.Length + copy.Length];
            for (int i = 0; i < (int)xstart; i++)
            {
                newfreq[i] = globalFreq[i];
            }
            for (int i = 0; i < copy.Length; i++)
            {
                newfreq[i + (int) xstart] = copy[i];
            }
            for (int i = (int)xstart + copy.Length; i < newfreq.Length; i++)
            {
                newfreq[i] = globalFreq[i - copy.Length];
            }
            globalFreq = newfreq;
            plotFreqWaveChart(globalFreq);
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            Copy_Click(sender, e);
            var list = globalFreq.ToList();
            list.RemoveRange((int)xstart, (int)xend - (int)xstart);
            globalFreq = list.ToArray();
            plotFreqWaveChart(globalFreq);
        }
    }
}
