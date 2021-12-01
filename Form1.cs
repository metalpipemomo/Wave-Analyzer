using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        [DllImport("RecordingDLL.dll", CallingConvention = CallingConvention.Cdecl)] public static extern void setBuffer(byte* newpbuffer, int length, int bps, int blockalign, int samplerate, int byterate, int nchannels);
        [DllImport("RecordingDLL.dll")] public static extern int getDwLength();
        [DllImport("RecordingDLL.dll")] public static extern IntPtr StartMessage();
        [DllImport("RecordingDLL.dll")] public static extern IntPtr StopMessage();
        [DllImport("RecordingDLL.dll")] public static extern IntPtr PlayMessage();
        [DllImport("RecordingDLL.dll")] public static extern IntPtr PlayStopMessage();

        private string filePath;
        private string fileName;
        private double[] globalFreq;
        private byte[] data;
        private double[] copy;
        private double xstart;
        private double xend;
        public static int freqinput;
        private bool player = false;
        private Color linecolor = Color.FromArgb(102, 255, 178);
        private Color selectioncolor = Color.FromArgb(140, 102, 255);
        private WavReader globalWavHdr = new WavReader();
        public Form1()
        {
            InitializeComponent();
            start();
            chartStyling();
            buttonStyling();
            Testing();

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,
                                                                       Color.FromArgb(255, 128, 0),
                                                                       Color.FromArgb(255, 51, 255),
                                                                       0F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
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
            ca.CursorX.SelectionColor = selectioncolor;
            cs.BorderWidth = 3;
            ca.BackColor = Color.Transparent;
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.LabelStyle.Enabled = false;
            ca.AxisY.MajorGrid.Enabled = false;
            this.chart1.MouseWheel += chart1_MouseWheel;
            hScrollBar1.Visible = false;
            ca.AxisX.ScaleView.Size = Constants.VIEWABLE;
        }

        private void buttonStyling()
        {
            Cut.Enabled = false;
            Copy.Enabled = false;
            Paste.Enabled = false;
            Cut.Visible = false;
            Copy.Visible = false;
            Paste.Visible = false;
            clearToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            hannWindowToolStripMenuItem1.Enabled = false;
            triangularWindowToolStripMenuItem1.Enabled = false;
            generateFilterToolStripMenuItem.Enabled = false;
            somethingToolStripMenuItem.Enabled = false;
            fileDetailsToolStripMenuItem.Enabled = false;
            StartPlay.Enabled = false;
            StartPlay.Visible = false;
        }

        public double[] readingWave(String fileName)
        {
            byte[] byteArray;
            BinaryReader reader = new BinaryReader(System.IO.File.OpenRead(fileName));
            globalWavHdr.Empty();
            int tracker = 0;
            globalWavHdr.ChunkID = reader.ReadInt32();
            tracker += 4;
            globalWavHdr.ChunkSize = reader.ReadInt32();
            tracker += 4;
            globalWavHdr.Format = reader.ReadInt32();
            tracker += 4;
            globalWavHdr.SubChunk1ID = reader.ReadInt32();
            tracker += 4;
            globalWavHdr.SubChunk1Size = reader.ReadInt32();
            tracker += 4;
            globalWavHdr.AudioFormat = reader.ReadUInt16();
            tracker += 2;
            globalWavHdr.NumChannels = reader.ReadUInt16();
            tracker += 2;
            globalWavHdr.SampleRate = reader.ReadUInt32();
            tracker += 4;
            globalWavHdr.ByteRate = reader.ReadUInt32();
            tracker += 4;
            globalWavHdr.BlockAlign = reader.ReadUInt16();
            tracker += 2;
            globalWavHdr.BitsPerSample = reader.ReadUInt16();
            tracker += 2;
            globalWavHdr.SubChunk2ID = reader.ReadInt32();
            tracker += 4;
            byte[] sum = BitConverter.GetBytes(globalWavHdr.SubChunk2ID);
            //read all bytes into byte array and find word data, index of d is where data starts so seek back to that position and read
            if (System.Text.Encoding.UTF8.GetString(sum) != "data")
            {
                string some = "";
                for (int i = 0; i < 5; ++i)
                {
                    byte[] b = { reader.ReadByte() };
                    some += System.Text.Encoding.UTF8.GetString(b);
                    tracker++;
                    switch (i)
                    {
                        case 1:
                            if (some != "d")
                            {
                                i = 0;
                                some = "";
                            }
                            break;
                        case 2:
                            if (some != "da")
                            {
                                i = 0;
                                some = "";
                            }
                            break;
                        case 3:
                            if (some != "dat")
                            {
                                i = 0;
                                some = "";
                            }
                            break;
                        case 4:
                            if (some != "data")
                            {
                                i = 0;
                                some = "";
                            }
                            break;
                        default:
                            break;
                    }
                    if (some == "data")
                    {
                        reader.BaseStream.Seek(tracker - 4, SeekOrigin.Begin);
                        globalWavHdr.SubChunk2ID = reader.ReadInt32();
                        break;
                    }
                }
            }
            globalWavHdr.SubChunk2Size = reader.ReadInt32();
            byteArray = reader.ReadBytes((int)globalWavHdr.SubChunk2Size);
            data = byteArray;
            fixed (byte* ptr = byteArray)
            {
                setBuffer(ptr, byteArray.Length, globalWavHdr.BitsPerSample, globalWavHdr.BlockAlign, (int)globalWavHdr.SampleRate, (int)globalWavHdr.ByteRate, globalWavHdr.NumChannels);
            }
            short[] shortArray = new short[globalWavHdr.SubChunk2Size / globalWavHdr.BlockAlign];
            double[] outputArray;
            for (int i = 0; i < globalWavHdr.SubChunk2Size / globalWavHdr.BlockAlign - 1; i++)
            {
                shortArray[i] = BitConverter.ToInt16(byteArray, i * globalWavHdr.BlockAlign);
            }
            outputArray = shortArray.Select(x => (double)(x)).ToArray();
            hScrollBar1.Maximum = outputArray.Length - Constants.VIEWABLE;
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
            chart1.Series[0].Points.Clear();
            int start = hScrollBar1.Value;
            int end = (int)chart1.ChartAreas[0].AxisX.ScaleView.Size + start;
            int index = 0;
            for (int i = start; i < end && i < array.Length; ++i)
            {
                chart1.Series[0].Points.AddXY(index, array[i]);
                ++index;
            }
            clearToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            hannWindowToolStripMenuItem1.Enabled = true;
            triangularWindowToolStripMenuItem1.Enabled = true;
            generateFilterToolStripMenuItem.Enabled = true;
            somethingToolStripMenuItem.Enabled = true;
            fileDetailsToolStripMenuItem.Enabled = true;
            hScrollBar1.Visible = true;
            StartPlay.Enabled = true;
            StartPlay.Visible = true;
            Cut.Enabled = true;
            Copy.Enabled = true;
            Cut.Visible = true;
            Copy.Visible = true;
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            xstart = chart1.ChartAreas[0].CursorX.SelectionStart + hScrollBar1.Value;
            xend = chart1.ChartAreas[0].CursorX.SelectionEnd + hScrollBar1.Value;
            Trace.WriteLine(xstart + "\n" + xend);
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            copy = new double[(int)xend - (int)xstart + 1];
            int nums = 0;
            for (int i = (int)xstart; i <= (int)xend; i++)
            {
                copy[nums] = globalFreq[i];
                nums++;
            }
            Paste.Enabled = true;
            Paste.Visible = true;
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
                newfreq[i + (int)xstart] = copy[i];
            }
            for (int i = (int)xstart + copy.Length; i < newfreq.Length; i++)
            {
                newfreq[i] = globalFreq[i - copy.Length];
            }
            globalFreq = newfreq;
            plotFreqWaveChart(globalFreq);
            byte[] newbuff = convertToByte(globalFreq);
            globalWavHdr.ChunkSize = Constants.CHUNK_SIZE + newbuff.Length;
            globalWavHdr.SubChunk2Size = newbuff.Length;
            Array.Resize(ref data, newbuff.Length);
            data = newbuff;
            fixed (byte* ptr = data)
            {
                setBuffer(ptr, data.Length, globalWavHdr.BitsPerSample, globalWavHdr.BlockAlign, (int)globalWavHdr.SampleRate, (int)globalWavHdr.ByteRate, globalWavHdr.NumChannels);
            }
            hScrollBar1.Maximum = globalFreq.Length - Constants.VIEWABLE;
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            Copy_Click(sender, e);
            var list = globalFreq.ToList();
            list.RemoveRange((int)xstart, (int)xend - (int)xstart);
            globalFreq = list.ToArray();
            plotFreqWaveChart(globalFreq);
            byte[] newbuff = convertToByte(globalFreq);
            globalWavHdr.ChunkSize = Constants.CHUNK_SIZE + newbuff.Length;
            globalWavHdr.SubChunk2Size = newbuff.Length;
            Array.Resize(ref data, newbuff.Length);
            data = newbuff;
            fixed (byte* ptr = data)
            {
                setBuffer(ptr, data.Length, globalWavHdr.BitsPerSample, globalWavHdr.BlockAlign, (int)globalWavHdr.SampleRate, (int)globalWavHdr.ByteRate, globalWavHdr.NumChannels);
            }
            hScrollBar1.Maximum = globalFreq.Length - Constants.VIEWABLE;
        }
        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            int init = (int)chart1.ChartAreas[0].AxisX.ScaleView.Size;
            double constant = e.Delta * 0.5;
            if (chart1.ChartAreas[0].AxisX.ScaleView.Size - constant > 0
                && chart1.ChartAreas[0].AxisX.ScaleView.Size - constant < globalFreq.Length
                && chart1.ChartAreas[0].AxisX.ScaleView.Size - constant + hScrollBar1.Value < globalFreq.Length)
            {
                chart1.ChartAreas[0].AxisX.ScaleView.Size -= constant;
                if (chart1.ChartAreas[0].AxisX.ScaleView.Size > init)
                {
                    for (int i = init; i < chart1.ChartAreas[0].AxisX.ScaleView.Size && i + hScrollBar1.Value < globalFreq.Length; ++i)
                    {
                        chart1.ChartAreas[0].AxisX.Maximum = i;
                        chart1.Series[0].Points.AddXY(i, globalFreq[i + hScrollBar1.Value]);
                    }
                }
                else
                {
                    for (int i = init - 1; i >= chart1.ChartAreas[0].AxisX.ScaleView.Size; --i)
                    {
                        chart1.Series[0].Points.RemoveAt(i);
                    }
                }
                hScrollBar1.Maximum = globalFreq.Length - (int)chart1.ChartAreas[0].AxisX.ScaleView.Size;
            }
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

        private void lowPassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FreqCut f = new FreqCut();
            double[] filter = new double[0];
            if (f.ShowDialog() == DialogResult.OK)
            {
                filter = Fourier.lowPassFilter(copy.Length, freqinput, globalWavHdr.SampleRate);
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
            fou.plotFourier(globalWavHdr.SampleRate, "Low-Pass");
            fou.Show();
            double[] megaexperiment = Fourier.convolve(globalFreq, idftdfw);
            globalFreq = megaexperiment;
            plotFreqWaveChart(globalFreq);
            data = convertToByte(globalFreq);
            fixed (byte* ptr = data)
            {
                setBuffer(ptr, data.Length, globalWavHdr.BitsPerSample, globalWavHdr.BlockAlign, (int)globalWavHdr.SampleRate, (int)globalWavHdr.ByteRate, globalWavHdr.NumChannels);
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
                setBuffer(ptr, data.Length, globalWavHdr.BitsPerSample, globalWavHdr.BlockAlign, (int)globalWavHdr.SampleRate, (int)globalWavHdr.ByteRate, globalWavHdr.NumChannels);
            }
        }

        private void Record_Click(object sender, EventArgs e)
        {
            if (!player)
            {
                StartMessage();
                player = true;
                Record.Image = Properties.Resources.stop;
            }
            else
            {
                StopMessage();
                player = false;
                Record.Image = Properties.Resources.microphone;
                data = new byte[getDwLength()];
                Marshal.Copy(getBuffer(), data, 0, getDwLength());
                short[] shortArray = new short[data.Length / 2];
                for (int i = 0; i < data.Length / 2; i++)
                {
                    shortArray[i] = BitConverter.ToInt16(data, i * 2);
                }
                globalFreq = shortArray.Select(x => (double)(x)).ToArray();
                plotFreqWaveChart(globalFreq);
                fixed (byte* ptr = data)
                {
                    setBuffer(ptr, data.Length, 16, 2, 44100, 88200, 1);
                }
                globalWavHdr.ChunkID = Constants.RIFF;
                globalWavHdr.ChunkSize = Constants.CHUNK_SIZE + data.Length;
                globalWavHdr.Format = Constants.WAVE;
                globalWavHdr.SubChunk1ID = Constants.fmt;
                globalWavHdr.SubChunk1Size = Constants.SUBCHUNK_1SIZE;
                globalWavHdr.AudioFormat = Constants.PCM;
                globalWavHdr.NumChannels = Constants.CHANNELS;
                globalWavHdr.SampleRate = Constants.SAMPLE_RATE;
                globalWavHdr.ByteRate = Constants.BYTE_RATE;
                globalWavHdr.BlockAlign = Constants.BLOCK_ALIGN;
                globalWavHdr.BitsPerSample = Constants.BITS_PER_SAMPLE;
                globalWavHdr.SubChunk2ID = Constants.data;
                globalWavHdr.SubChunk2Size = data.Length;
                hScrollBar1.Maximum = globalFreq.Length - Constants.VIEWABLE;
                StartPlay.Enabled = true;
                StartPlay.Visible = true;
            }
        }

        private void StartPlay_Click(object sender, EventArgs e)
        {
            if (!player)
            {
                PlayMessage();
                player = true;
                Trace.WriteLine("Existing");
                StartPlay.Image = Properties.Resources.pause;
            }
            else
            {
                PlayStopMessage();
                player = false;
                Trace.WriteLine("Existn't");
                StartPlay.Image = Properties.Resources.play;
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
                this.Text = openFileDialog.SafeFileName;
                filePath = openFileDialog.SafeFileName;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //add filter
            saveFileDialog.Filter = "WAV File (*.wav)|*.wav|All files (*.*)|*.*";
            saveFileDialog.DefaultExt = "wav";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = (FileStream)saveFileDialog.OpenFile();
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
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            buttonStyling();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //if scaleview size + scrollvalue < wavelength
            if (chart1.ChartAreas[0].AxisX.ScaleView.Size + hScrollBar1.Value < globalFreq.Length)
            {
                plotFreqWaveChart(globalFreq);
            }
        }

        private void plainDFTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakePlainDFT();
        }

        private void hannWindowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MakeHann();
        }

        private void triangularWindowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MakeTriangle();
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stopwatch watch1 = new Stopwatch();
            Stopwatch watch2 = new Stopwatch();
            Stopwatch watch3 = new Stopwatch();
            double[] hann = Fourier.hannWindow(copy);
            FourierForm f = new FourierForm();
            double[] triangle = Fourier.triangleWindow(copy);
            FourierForm f1 = new FourierForm();
            double[] samples = copy;
            FourierForm f2 = new FourierForm();
            Thread t = new Thread(() => f.doFourier(hann));
            Thread t1 = new Thread(() => f1.doFourier(triangle));
            Thread t2 = new Thread(() => f2.doFourier(samples));
            watch1.Start();
            t.Start();
            watch2.Start();
            t1.Start();
            watch3.Start();
            t2.Start();
            t.Join();
            watch1.Stop();
            t1.Join();
            watch2.Stop();
            t2.Join();
            watch3.Stop();
            f.timelapsed = watch1.ElapsedMilliseconds;
            f1.timelapsed = watch2.ElapsedMilliseconds;
            f2.timelapsed = watch3.ElapsedMilliseconds;
            f.plotFourier(globalWavHdr.SampleRate, "Hann Window");
            f.Show();
            f1.plotFourier(globalWavHdr.SampleRate, "Triangle Window");
            f1.Show();
            f2.plotFourier(globalWavHdr.SampleRate, "DFT");
            f2.Show();
        }

        public void MakePlainDFT()
        {
            Stopwatch watch = new Stopwatch();
            double[] samples = copy;
            FourierForm f = new FourierForm();
            Thread t = new Thread(() => f.doFourier(samples));
            watch.Start();
            t.Start();
            t.Join();
            watch.Stop();
            f.timelapsed = watch.ElapsedMilliseconds;
            f.plotFourier(globalWavHdr.SampleRate, "DFT");
            f.Show();
        }

        public void MakeHann()
        {
            Stopwatch watch = new Stopwatch();
            double[] hann = Fourier.hannWindow(copy);
            FourierForm f = new FourierForm();
            Thread t = new Thread(() => f.doFourier(hann));
            watch.Start();
            t.Start();
            t.Join();
            watch.Stop();
            f.timelapsed = watch.ElapsedMilliseconds;
            f.plotFourier(globalWavHdr.SampleRate, "Hann Window");
            f.Show();
        }

        public void MakeTriangle()
        {
            Stopwatch watch = new Stopwatch();
            double[] triangle = Fourier.triangleWindow(copy);
            FourierForm f = new FourierForm();
            Thread t = new Thread(() => f.doFourier(triangle));
            watch.Start();
            t.Start();
            t.Join();
            watch.Stop();
            f.timelapsed = watch.ElapsedMilliseconds;
            f.plotFourier(globalWavHdr.SampleRate, "Triangle Window");
            f.Show();
        }

        private void fileDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InfoForm f = new InfoForm(filePath, globalWavHdr);
            f.Show();
        }
    }
}
