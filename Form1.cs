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
using System.Threading;

namespace WaveAnalyzer
{
    public unsafe partial class Form1 : Form
    {
        //need to marshal or something to get hinstance and pstr
        [DllImport("Record.dll")] public static extern int start();
        [DllImport("Record.dll")] public static extern IntPtr getBuffer();
        [DllImport("Record.dll", CallingConvention = CallingConvention.Cdecl)] public static extern void setBuffer(byte* newpbuffer);
        [DllImport("Record.dll")] public static extern int getDwLength();
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
            this.Text = "Mo's Wave Analyzer";
            chartStyling();
            buttonStyling();
            
        }

        private void chartStyling()
        {
            var ca = chart1.ChartAreas[0];
            var cs = chart1.Series["Original"];
            cs.ChartType = SeriesChartType.FastLine;
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
            /*fixed (byte* ptr = byteArray)
            {
                setBuffer(ptr);
            }*/
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
            //Text = fileName;
            globalFreq = readingWave(filePath);
            plotFreqWaveChart(globalFreq);
        }

        public void plotFreqWaveChart(double[] array)
        {
            chart1.Series["Original"].Points.Clear();
            for (int i = 0; i < array.Length / 4; i++)
            {
                chart1.Series["Original"].Points.AddXY(i, array[i]);
            }
            for (int i = array.Length / 4; i < array.Length - array.Length / 2 ; i++)
            {
                chart1.Series["Original"].Points.AddXY(i, array[i]);
            }
            for (int i = array.Length / 2; i < array.Length - array.Length / 4; i++)
            {
                chart1.Series["Original"].Points.AddXY(i, array[i]);
            }
            for (int i = array.Length - array.Length / 4; i < array.Length; i++)
            {
                chart1.Series["Original"].Points.AddXY(i, array[i]);
            }
            chart1.ChartAreas[0].AxisX.ScaleView.Size = array.Length / 25;
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
            FileStream fs = new FileStream("C:\\Users\\banga\\Downloads\\Written.wav", FileMode.CreateNew);
            BinaryWriter writer = new BinaryWriter(fs);
            writer.Write("RIFF"); //RIFF
            writer.Write(); //File Size (integer)
            writer.Write("WAVE"); //WAVE
            writer.Write("fmt");//fmt
            //

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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private class ZoomFrame
        {
            public double XStart { get; set; }
            public double XFinish { get; set; }
        }
        private readonly Stack<ZoomFrame> _zoomFrames = new Stack<ZoomFrame>();
        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            xAxis.ScaleView.Zoomable = true;
            if (e.Delta < 0)
            {
                if (0 < _zoomFrames.Count)
                {
                    var frame = _zoomFrames.Pop();
                    if (_zoomFrames.Count == 0)
                    {
                        xAxis.ScaleView.ZoomReset();
                        xAxis.ScaleView.Size = globalFreq.Length / 25;
                    } 
                    else
                    {
                        xAxis.ScaleView.Zoom(frame.XStart, frame.XFinish);
                    }
                }
            }
            else if (e.Delta > 0)
            {
                var xMin = xAxis.ScaleView.ViewMinimum;
                var xMax = xAxis.ScaleView.ViewMaximum;
                _zoomFrames.Push(new ZoomFrame { XStart = xMin, XFinish = xMax});
                var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
                var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;
                xAxis.ScaleView.Zoom(posXStart, posXFinish);
            }
            xAxis.ScaleView.Zoomable = false;
        }
        public void Testing()
        {
            double[] s = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            double[] fw = { 1, 0.2, 0.2, 0, 1, 0.1, 0.1, 1 };
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
            double[] shannonentropytest = { 5, 10, 15, 10, 8, 10, 15, 5, 8, 3, 8, 5, 8, 10, 10, 15, 10, 10, 8, 5, 5 };
            Trace.WriteLine("Shannon Entropy Test:");
            Trace.WriteLine(Fourier.entropy(shannonentropytest));
            Fourier.printSamplesTrace(Fourier.uniquearr(shannonentropytest));
            Fourier.printSamplesTrace(Fourier.inverseDFT(Fourier.convertFilter(myfilter), myfilter.Length));
        }
        public byte[] toByteArr(double[] d)
        {
            short[] shortArray = new short[globalWavHdr.SubChunk2Size / globalWavHdr.BlockAlign];
            byte[] bytearr = new byte[shortArray.Length * 2];
            shortArray = globalFreq.Select(x => (short)(x)).ToArray();
            bytearr = Array.ConvertAll(new Converter<short, byte>(shortArray));
        }
    }
}
