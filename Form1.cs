using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveAnalyzer
{
    public unsafe partial class Form1 : Form
    {
        [DllImport("RecordingDLL.dll")] public static extern int start();
        [DllImport("RecordingDLL.dll")] public static extern IntPtr getBuffer();
        [DllImport("RecordingDLL.dll", CallingConvention = CallingConvention.Cdecl)] public static extern void setBuffer(byte* newpbuffer, int length, int bps, int blockalign, int samplerate, int byterate);
        [DllImport("RecordingDLL.dll")] public static extern int getDwLength();
        [DllImport("RecordingDLL.dll")] public static extern IntPtr StartMessage();
        [DllImport("RecordingDLL.dll")] public static extern IntPtr StopMessage();
        [DllImport("RecordingDLL.dll")] public static extern IntPtr PlayMessage();
        [DllImport("RecordingDLL.dll")] public static extern IntPtr PlayStopMessage();
        private string filePath;
        private double[] globalFreq;
        private byte[] data;
        private double[] copy;
        private double xstart;
        private double xend;
        public static int freqinput;
        private bool player = false;
        private Color linecolor = Color.FromArgb(255, 105, 180);
        private WavReader globalWavHdr = new WavReader();
        public Form1()
        {
            InitializeComponent();
            start();
            chartStyling();
            buttonStyling();
            Testing();
            
        }

        private void chartStyling()
        {
            var ca = chart1.ChartAreas[0];
            var cs = chart1.Series["Original"];
            cs.ChartType = SeriesChartType.FastLine;
            ca.AxisX.Minimum = 0;
            ca.AxisX.Maximum = Double.NaN;
            ca.AxisX.ScrollBar.Enabled = false;
            ca.AxisX.ScaleView.Zoomable = false;
            ca.CursorX.IsUserEnabled = true;
            ca.CursorX.IsUserSelectionEnabled = true;
            cs.Color = linecolor;
            ca.CursorX.SelectionColor = linecolor;
            ca.BackColor = Color.SlateGray;
            ca.AxisX.ScrollBar.BackColor = Color.White;
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisY.MajorGrid.Enabled = false;
            this.chart1.MouseWheel += chart1_MouseWheel;
            hScrollBar1.Visible = false;
        }

        private void buttonStyling()
        {
            Cut.Enabled = false;
            Copy.Enabled = false;
            Paste.Enabled = false;
            clearToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            hannWindowToolStripMenuItem.Enabled = false;
            triangularWindowToolStripMenuItem.Enabled = false;
            generateFilterToolStripMenuItem.Enabled = false;
            somethingToolStripMenuItem.Enabled = false;
            StartPlay.Enabled = false;
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
            data = byteArray;
            fixed (byte* ptr = byteArray)
            {
                setBuffer(ptr, byteArray.Length, globalWavHdr.BitsPerSample, globalWavHdr.BlockAlign, (int)globalWavHdr.SampleRate, (int)globalWavHdr.ByteRate);
            }
            short[] shortArray = new short[globalWavHdr.SubChunk2Size / globalWavHdr.BlockAlign];
            double[] outputArray;
            for (int i = 0; i < globalWavHdr.SubChunk2Size / globalWavHdr.BlockAlign; i++)
            {
                shortArray[i] = BitConverter.ToInt16(byteArray, i * globalWavHdr.BlockAlign);
            }
            outputArray = shortArray.Select(x => (double)(x)).ToArray();
            hScrollBar1.Maximum = outputArray.Length - (outputArray.Length / 25);
            Trace.WriteLine(outputArray.Length);
            reader.Close();
            return outputArray;
        }

        public void OpenFile(string fileName)
        {
            filePath = fileName;
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
            for (int i = array.Length / 4; i < array.Length - array.Length / 2; i++)
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
            /**
             * store scroll start
             * store scroll end (the size of your scaleview + start)
             * set an index (?)
             * from scroll start, while its less than scroll end and less than wave length, increment
             * do:
             * add xy index, and data at index
             * increment index
             */
            /*int start = hScrollBar1.Value;
            int end = (int) chart1.ChartAreas[0].AxisX.ScaleView.Size + start;
            int index = 0;
            for (int i = start; i < end && i < globalFreq.Length; ++i)
            {
                chart1.Series[0].Points.AddXY(index, globalFreq[i]);
                ++index;
            }*/
            chart1.ChartAreas[0].AxisX.ScaleView.Size = array.Length / 25;
            clearToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            hannWindowToolStripMenuItem.Enabled = true;
            triangularWindowToolStripMenuItem.Enabled = true;
            generateFilterToolStripMenuItem.Enabled = true;
            somethingToolStripMenuItem.Enabled = true;
            hScrollBar1.Visible = true;
            StartPlay.Enabled = true;
            Cut.Enabled = true;
            Copy.Enabled = true;
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            xstart = chart1.ChartAreas[0].CursorX.SelectionStart;
            xend = chart1.ChartAreas[0].CursorX.SelectionEnd;
            Trace.WriteLine(xstart + "\n" + xend);
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

        private void hannWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[] hann = Fourier.hannWindow(copy);
            FourierForm f = new FourierForm();
            Thread t = new Thread(() => f.doFourier(hann));
            t.Start();
            t.Join();
            f.plotFourier(globalWavHdr.SampleRate, "Hann Window");
            f.Show();
        }

        private void triangularWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[] triangle = Fourier.triangleWindow(copy);
            FourierForm f = new FourierForm();
            Thread t = new Thread(() => f.doFourier(triangle));
            t.Start();
            t.Join();
            f.plotFourier(globalWavHdr.SampleRate, "Triangle Window");
            f.Show();
        }

        private void lowPassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FreqCut f = new FreqCut();
            double[] filter = new double[copy.Length];
            if (f.ShowDialog() == DialogResult.OK)
            {
                filter = Fourier.lowPassFilter(copy.Length, freqinput, globalWavHdr.SampleRate);
                Fourier.printSamplesTrace(filter);
            } else
            {
                return;
            }
            Complex[] fw = Fourier.convertFilter(filter);
            double[] idftdfw = Fourier.inverseDFT(fw, fw.Length);
            double[] newsamples = Fourier.convolve(copy, idftdfw);
            copy = newsamples;
            FourierForm fou = new FourierForm();
            Thread t = new Thread(() => fou.doFourier(newsamples));
            t.Start();
            t.Join();
            fou.plotFourier(globalWavHdr.SampleRate, "Low-Pass");
            fou.Show();
            double[] megaexperiment = Fourier.convolve(globalFreq, idftdfw);
            globalFreq = megaexperiment;
            plotFreqWaveChart(globalFreq);
            data = convertToByte(globalFreq);
            fixed (byte* ptr = data)
            {
                setBuffer(ptr, data.Length, globalWavHdr.BitsPerSample, globalWavHdr.BlockAlign, (int)globalWavHdr.SampleRate, (int)globalWavHdr.ByteRate);
            }
        }

        private void highPassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FreqCut f = new FreqCut();
            double[] filter = new double[0];
            if (f.ShowDialog() == DialogResult.OK)
            {
                filter = Fourier.highPassFilter(copy.Length, freqinput, globalWavHdr.SampleRate);
                Fourier.printSamplesTrace(filter);
            }
            Complex[] fw = Fourier.convertFilter(filter);
            double[] idftdfw = Fourier.inverseDFT(fw, fw.Length);
            double[] newsamples = Fourier.convolve(copy, idftdfw);
            copy = newsamples;
            FourierForm fou = new FourierForm();
            Thread t = new Thread(() => fou.doFourier(newsamples));
            t.Start();
            t.Join();
            fou.plotFourier(globalWavHdr.SampleRate, "High-Pass");
            fou.Show();
            double[] megaexperiment = Fourier.convolve(globalFreq, idftdfw);
            globalFreq = megaexperiment;
            plotFreqWaveChart(globalFreq);
            data = convertToByte(globalFreq);
            fixed (byte* ptr = data)
            {
                setBuffer(ptr, data.Length, globalWavHdr.BitsPerSample, globalWavHdr.BlockAlign, (int)globalWavHdr.SampleRate, (int)globalWavHdr.ByteRate);
            }
        }

        private void Record_Click(object sender, EventArgs e)
        {
            if (!player)
            {
                StartMessage();
                player = true;
            } else
            {
                StopMessage();
                player = false;
                data = new byte[getDwLength()];
                Marshal.Copy(getBuffer(), data, 0, getDwLength());
                short[] shortArray = new short[data.Length / 2];
                double[] outputArray;
                for (int i = 0; i < data.Length / 2; i++)
                {
                    shortArray[i] = BitConverter.ToInt16(data, i * 2);
                }
                globalFreq = shortArray.Select(x => (double)(x)).ToArray();
                plotFreqWaveChart(globalFreq);
                fixed (byte* ptr = data)
                {
                    setBuffer(ptr, data.Length, 16, 2, 44100, 88200);
                }
                globalWavHdr.BlockAlign = 2;
                globalWavHdr.BitsPerSample = 16;
                globalWavHdr.SampleRate = 44100;
                globalWavHdr.ByteRate = 88200;
                StartPlay.Enabled = true;
            }
        }

        private void StartPlay_Click(object sender, EventArgs e)
        {
            if (!player)
            {
                PlayMessage();
                player = true;
                Trace.WriteLine("Existing");
            } else
            {
                PlayStopMessage();
                player = false;
                Trace.WriteLine("Existn't");
            }
        }

        public byte[] convertToByte(double[] s)
        {
            short[] news = new short[s.Length];
            news = s.Select(x => (short)(x)).ToArray();
            List<Byte> b = new List<byte>();
            for (int i = 0; i < news.Length; ++i)
            {
                b.AddRange(BitConverter.GetBytes(news[i]).ToList());
            }
            return b.ToArray();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            FileStream fs = new FileStream("C:\\Users\\banga\\Downloads\\Written" + rand.Next() + ".wav", FileMode.CreateNew);
            BinaryWriter writer = new BinaryWriter(fs);
            writer.Write(globalWavHdr.ChunkID); //RIFF
            writer.Write(globalWavHdr.ChunkSize); //File Size (integer)
            writer.Write(globalWavHdr.Format); //WAVE
            writer.Write(globalWavHdr.SubChunk1ID);//fmt
            writer.Write(globalWavHdr.SubChunk1Size); //length of above data
            writer.Write(globalWavHdr.AudioFormat); //PCM
            writer.Write(globalWavHdr.NumChannels); //Channel numbers
            writer.Write(globalWavHdr.SampleRate); //Sample rate
            writer.Write(globalWavHdr.ByteRate); //Byte rate
            writer.Write(globalWavHdr.BlockAlign);
            writer.Write(globalWavHdr.BitsPerSample);
            writer.Write(globalWavHdr.SubChunk2ID);
            writer.Write(globalWavHdr.SubChunk2Size);
            writer.Write(data);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series["Original"].Points.Clear();
            buttonStyling();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //if scaleview size + scrollvalue < wavelength
            /*if (chart1.ChartAreas[0].AxisX.ScaleView.Size + hScrollBar1.Value < globalFreq.Length)
            {
                plotFreqWaveChart(globalFreq);
            }*/
        }
    }
}
