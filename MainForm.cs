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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveAnalyzer
{
    public unsafe partial class MainForm : Form
    {
        
        private string filePath; //stores file path
        private byte[] fileData; //stores samples in byte form
        private double[] copy;   //stores copied samples
        private double xStart;   //start of selection on axis X
        private double xEnd;     //end of selection on axis X
        private bool playing;    //for handling recording/playing start and stop messages
        private bool scrollable; //allow scrolling
        private Color lineColor = Color.FromArgb(102, 255, 178);        //color for points on the wave
        private Color selectionColor = Color.FromArgb(140, 102, 255);   //color for selection on the wave
        private WavReader waveHeader = new WavReader();                 //Wave header
        public double[] fileSamples;                                    //Stores all samples, used heavily

        /// <summary>
        /// Main Form constructor.
        /// Initializes DLL and chart and button styling.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            RecordingFunctions.start();
            chartStyling();
            buttonStyling();
        }

        /// <summary>
        /// Overrides paint message.
        /// I am using this to color the background using a gradient.
        /// </summary>
        /// <param name="e">I believe this is the paint message.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,
                                                                       Color.FromArgb(255, 128, 0),
                                                                       Color.FromArgb(255, 51, 255),
                                                                       45F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        /// <summary>
        /// Reads in a wave using a Binary Reader and stores in waveHeader class.<br/>
        /// Will check for garbage values in between bits per sample and subchunk2id.
        /// </summary>
        /// <param name="fileName">Path to file for BinaryReader to open.</param>
        /// <returns>Array of samples stored as doubles.</returns>
        public double[] readingWave(string fileName)
        {
            byte[] byteArray;
            BinaryReader reader = new BinaryReader(System.IO.File.OpenRead(fileName));
            waveHeader.Empty();
            waveHeader.ChunkID = reader.ReadInt32();
            waveHeader.ChunkSize = reader.ReadInt32();
            waveHeader.Format = reader.ReadInt32();
            waveHeader.SubChunk1ID = reader.ReadInt32();
            waveHeader.SubChunk1Size = reader.ReadInt32();
            waveHeader.AudioFormat = reader.ReadUInt16();
            waveHeader.NumChannels = reader.ReadUInt16();
            waveHeader.SampleRate = reader.ReadUInt32();
            waveHeader.ByteRate = reader.ReadUInt32();
            waveHeader.BlockAlign = reader.ReadUInt16();
            waveHeader.BitsPerSample = reader.ReadUInt16();
            //Finds the word "data" after finding bits per sample.
            //Doing this error checking because sometimes there are garbage values between bps and subchunk2id.
            byte[] test = new byte[Constants.MAXHEADERSIZE];
            Array.Copy(reader.ReadBytes(Constants.MAXHEADERSIZE), test, test.Length);
            int offset = 0;
            for (; offset < test.Length; ++offset) {
                string s = "";
                bool offsetfound = false;
                for (int j = 0; j < 4; ++j) {
                    s += System.Text.Encoding.UTF8.GetString(new byte[] { test[offset + j] });
                    if (s == "data") {
                        offsetfound = true;
                        break;
                    }
                }
                if (offsetfound) break;
            }
            //Move reader to the position of where data was found.
            reader.BaseStream.Seek(36 + offset, SeekOrigin.Begin);
            waveHeader.SubChunk2ID = reader.ReadInt32();
            waveHeader.SubChunk2Size = reader.ReadInt32();
            byteArray = new byte[waveHeader.SubChunk2Size];
            byte[] temp = reader.ReadBytes(waveHeader.SubChunk2Size);
            Array.Copy(temp, byteArray, temp.Length);
            fileData = byteArray;
            fixed (byte* ptr = byteArray)
            {
                RecordingFunctions.setBuffer(ptr, byteArray.Length, waveHeader.BitsPerSample, waveHeader.BlockAlign, (int)waveHeader.SampleRate, (int)waveHeader.ByteRate, waveHeader.NumChannels);
            }
            short[] shortArray = new short[waveHeader.SubChunk2Size / waveHeader.BlockAlign];
            double[] outputArray;
            //Check if bits per sample is less than 16 or basically 8 bits.
            //We want to basically just cast our bytes to shorts in the case where they are 8 bits.
            if (waveHeader.BitsPerSample < 16)
            {
                for (int i = 0; i < waveHeader.SubChunk2Size / waveHeader.BlockAlign - 1; i++) {
                    shortArray[i] = (short)byteArray[i];
                }
            }
            else
            {
                for (int i = 0; i < waveHeader.SubChunk2Size / waveHeader.BlockAlign - 1; i++)
                {
                    shortArray[i] = BitConverter.ToInt16(byteArray, i * waveHeader.BlockAlign);
                }
            }
            
            outputArray = shortArray.Select(x => (double)(x)).ToArray();
            hScrollBar1.Maximum = outputArray.Length - Constants.VIEWABLE;
            reader.Close();
            return outputArray;
        }

        /// <summary>
        /// Assigns filePath to parameter.<br/>
        /// Reads and plots wav file.
        /// </summary>
        /// <param name="fileName">Path to file to be opened by readingWave().</param>
        public void openFile(string fileName)
        {
            filePath = fileName;
            fileSamples = readingWave(filePath);
            plotFreqWaveChart(fileSamples);
        }

        /// <summary>
        /// Plots given array of doubles.<br/>
        /// Optimized to only draw whats on the screen.
        /// </summary>
        /// <param name="array">Samples to be drawn.</param>
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
            enableButtons();
            scrollable = true;
        }

        /// <summary>
        /// Stores the start and end of the selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart1_Click(object sender, EventArgs e)
        {
            xStart = chart1.ChartAreas[0].CursorX.SelectionStart + hScrollBar1.Value;
            xEnd = chart1.ChartAreas[0].CursorX.SelectionEnd + hScrollBar1.Value;
        }

        /// <summary>
        /// Copies elements from indices xStart to xEnd and stores them in copy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Copy_Click(object sender, EventArgs e)
        {
            if (xStart > xEnd) {
                int temp = (int)xEnd;
                xEnd = xStart;
                xStart = temp;
            }
            copy = new double[(int)xEnd - (int)xStart + 1];
            int nums = 0;
            for (int i = (int)xStart; i <= (int)xEnd; i++)
            {
                copy[nums] = fileSamples[i];
                nums++;
            }
            Paste.Enabled = true;
            Paste.Visible = true;
        }

        /// <summary>
        /// Pastes elements back into fileSamples and redraws the wave.<br/>
        /// Sets the buffer for playback.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Paste_Click(object sender, EventArgs e)
        {
            double[] newfreq = new double[fileSamples.Length + copy.Length];
            for (int i = 0; i < (int)xStart; i++)
            {
                newfreq[i] = fileSamples[i];
            }
            for (int i = 0; i < copy.Length; i++)
            {
                newfreq[i + (int)xStart] = copy[i];
            }
            for (int i = (int)xStart + copy.Length; i < newfreq.Length; i++)
            {
                newfreq[i] = fileSamples[i - copy.Length];
            }
            fileSamples = newfreq;
            plotFreqWaveChart(fileSamples);
            byte[] newbuff = convertToByte(fileSamples);
            waveHeader.ChunkSize = Constants.CHUNK_SIZE + newbuff.Length;
            waveHeader.SubChunk2Size = newbuff.Length;
            Array.Resize(ref fileData, newbuff.Length);
            fileData = newbuff;
            fixed (byte* ptr = fileData)
            {
                RecordingFunctions.setBuffer(ptr, fileData.Length, waveHeader.BitsPerSample, waveHeader.BlockAlign, (int)waveHeader.SampleRate, (int)waveHeader.ByteRate, waveHeader.NumChannels);
            }
            hScrollBar1.Maximum = fileSamples.Length - Constants.VIEWABLE;
        }

        /// <summary>
        /// Removes selected samples from fileSamples and copies them into copy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cut_Click(object sender, EventArgs e)
        {
            Copy_Click(sender, e);
            var list = fileSamples.ToList();
            list.RemoveRange((int)xStart, (int)xEnd - (int)xStart);
            fileSamples = list.ToArray();
            plotFreqWaveChart(fileSamples);
            byte[] newbuff = convertToByte(fileSamples);
            waveHeader.ChunkSize = Constants.CHUNK_SIZE + newbuff.Length;
            waveHeader.SubChunk2Size = newbuff.Length;
            Array.Resize(ref fileData, newbuff.Length);
            fileData = newbuff;
            fixed (byte* ptr = fileData)
            {
                RecordingFunctions.setBuffer(ptr, fileData.Length, waveHeader.BitsPerSample, waveHeader.BlockAlign, (int)waveHeader.SampleRate, (int)waveHeader.ByteRate, waveHeader.NumChannels);
            }
            hScrollBar1.Maximum = fileSamples.Length - Constants.VIEWABLE;
        }

        /// <summary>
        /// Zooming, adjust constant to speed up. <br/>
        /// Decreases scaleview and draws in more points.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            int InitialSizeOfView = (int)chart1.ChartAreas[0].AxisX.ScaleView.Size;
            double constant = e.Delta * 3;
            if (scrollable && chart1.ChartAreas[0].AxisX.ScaleView.Size - constant > 0
                && chart1.ChartAreas[0].AxisX.ScaleView.Size - constant < fileSamples.Length
                && chart1.ChartAreas[0].AxisX.ScaleView.Size - constant + hScrollBar1.Value < fileSamples.Length)
            {
                chart1.ChartAreas[0].AxisX.ScaleView.Size -= constant;
                if (chart1.ChartAreas[0].AxisX.ScaleView.Size > InitialSizeOfView)
                {
                    for (int i = InitialSizeOfView; i < chart1.ChartAreas[0].AxisX.ScaleView.Size && i + hScrollBar1.Value < fileSamples.Length; ++i)
                    {
                        chart1.ChartAreas[0].AxisX.Maximum = i;
                        chart1.Series[0].Points.AddXY(i, fileSamples[hScrollBar1.Value + i]);
                    }
                }
                else
                {
                    for (int i = InitialSizeOfView - 1; i >= chart1.ChartAreas[0].AxisX.ScaleView.Size; --i)
                    {
                        chart1.Series[0].Points.RemoveAt(i);
                    }
                }
                hScrollBar1.Maximum = fileSamples.Length - (int)chart1.ChartAreas[0].AxisX.ScaleView.Size;
            }
        }

        /// <summary>
        /// Starts/stops recording. <br/>
        /// Sets header data and plots wave.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Record_Click(object sender, EventArgs e)
        {
            if (!playing)
            {
                RecordingFunctions.StartMessage();
                playing = true;
                Record.Image = Properties.Resources.stop;
            }
            else
            {
                RecordingFunctions.StopMessage();
                playing = false;
                Record.Image = Properties.Resources.microphone;
                fileData = new byte[RecordingFunctions.getDwLength()];
                Marshal.Copy(RecordingFunctions.getBuffer(), fileData, 0, RecordingFunctions.getDwLength());
                short[] shortArray = new short[fileData.Length / 2];
                for (int i = 0; i < fileData.Length / 2; i++)
                {
                    shortArray[i] = BitConverter.ToInt16(fileData, i * 2);
                }
                fileSamples = shortArray.Select(x => (double)(x)).ToArray();
                plotFreqWaveChart(fileSamples);
                fixed (byte* ptr = fileData)
                {
                    RecordingFunctions.setBuffer(ptr, fileData.Length, 16, 2, 44100, 88200, 1);
                }
                waveHeader.ChunkID = Constants.RIFF;
                waveHeader.ChunkSize = Constants.CHUNK_SIZE + fileData.Length;
                waveHeader.Format = Constants.WAVE;
                waveHeader.SubChunk1ID = Constants.fmt;
                waveHeader.SubChunk1Size = Constants.SUBCHUNK_1SIZE;
                waveHeader.AudioFormat = Constants.PCM;
                waveHeader.NumChannels = Constants.CHANNELS;
                waveHeader.SampleRate = Constants.SAMPLE_RATE;
                waveHeader.ByteRate = Constants.BYTE_RATE;
                waveHeader.BlockAlign = Constants.BLOCK_ALIGN;
                waveHeader.BitsPerSample = Constants.BITS_PER_SAMPLE;
                waveHeader.SubChunk2ID = Constants.data;
                waveHeader.SubChunk2Size = fileData.Length;
                hScrollBar1.Maximum = fileSamples.Length - Constants.VIEWABLE;
                StartPlay.Enabled = true;
                StartPlay.Visible = true;
                scrollable = true;
            }
        }

        /// <summary>
        /// Starts/stops playing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartPlay_Click(object sender, EventArgs e) {
            if (!playing) {
                RecordingFunctions.PlayMessage();
                playing = true;
                Trace.WriteLine("Existing");
                StartPlay.Image = Properties.Resources.pause;
            }
            else {
                RecordingFunctions.PlayStopMessage();
                playing = false;
                Trace.WriteLine("Existn't");
                StartPlay.Image = Properties.Resources.play;
            }
        }

        /// <summary>
        /// Opens OpenFileDialog to retrieve a wav file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Basically the menu where you select files
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //add filter
            openFileDialog.Filter = "WAV File (*.wav)|*.wav|All files (*.*)|*.*";
            //check if file open
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                openFile(openFileDialog.FileName);
                this.Text = openFileDialog.SafeFileName;
                filePath = openFileDialog.SafeFileName;
            }
        }

        /// <summary>
        /// Saves a wave as a new file by creating a new file and writing to it using BinaryWriter. <br/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                writer.Write(waveHeader.ChunkID); //RIFF
                writer.Write(waveHeader.ChunkSize); //File Size (integer)
                writer.Write(waveHeader.Format); //WAVE
                writer.Write(waveHeader.SubChunk1ID);//fmt
                writer.Write(waveHeader.SubChunk1Size); //length of above data
                writer.Write(waveHeader.AudioFormat); //PCM
                writer.Write(waveHeader.NumChannels); //Channel numbers
                writer.Write(waveHeader.SampleRate); //Sample rate
                writer.Write(waveHeader.ByteRate); //Byte rate
                writer.Write(waveHeader.BlockAlign);
                writer.Write(waveHeader.BitsPerSample);
                writer.Write(waveHeader.SubChunk2ID);
                writer.Write(waveHeader.SubChunk2Size);
                writer.Write(fileData);
            }
        }

        /// <summary>
        /// Clears the wave.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            buttonStyling();
            scrollable = false;
        }

        /// <summary>
        /// Plots wave on scroll. This is how rendering only visible points is achieved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (chart1.ChartAreas[0].AxisX.ScaleView.Size + hScrollBar1.Value < fileSamples.Length)
            {
                plotFreqWaveChart(fileSamples);
            }
        }

        /// <summary>
        /// Creates a regular DFT from selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void plainDFTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakePlainDFT();
        }

        /// <summary>
        /// Creates a hann windowed DFT from selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hannWindowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MakeHann();
        }

        /// <summary>
        /// Creates a triangle windowed DFT from selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void triangularWindowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MakeTriangle();
        }

        /// <summary>
        /// Creates all DFTs windowed or not. <br/>
        /// Windows are created concurrently and will run a threaded and unthreaded DFT from selection. <br/>
        /// Each window runs a threaded and unthreaded DFT for benchmarking purposes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stopwatch watch1 = new Stopwatch();
            Stopwatch watch2 = new Stopwatch();
            Stopwatch watch3 = new Stopwatch();
            Stopwatch watch4 = new Stopwatch();
            Stopwatch watch5 = new Stopwatch();
            Stopwatch watch6 = new Stopwatch();
            double[] hann = Fourier.hannWindow(copy);
            FourierForm f = new FourierForm(this);
            double[] triangle = Fourier.triangleWindow(copy);
            FourierForm f1 = new FourierForm(this);
            double[] samples = copy;
            FourierForm f2 = new FourierForm(this);
            f.whichfourier = 1;
            f1.whichfourier = 1;
            f2.whichfourier = 1;
            watch1.Start();
            Task t = Task.Factory.StartNew(() => f.doFourier(hann));
            watch2.Start();
            Task t1 = Task.Factory.StartNew(() => f1.doFourier(triangle));
            watch3.Start();
            Task t2 = Task.Factory.StartNew(() => f2.doFourier(samples));
            Task.WaitAll(t, t1, t2);
            watch1.Stop();
            watch2.Stop();
            watch3.Stop();
            f.whichfourier = 2;
            f1.whichfourier = 2;
            f2.whichfourier = 2;
            watch4.Start();
            Task t4 = Task.Factory.StartNew(() => f.doFourier(hann));
            watch5.Start();
            Task t5 = Task.Factory.StartNew(() => f1.doFourier(triangle));
            watch6.Start();
            Task t6 = Task.Factory.StartNew(() => f2.doFourier(samples));
            Task.WaitAll(t4, t5, t6);
            watch4.Stop();
            watch5.Stop();
            watch6.Stop();
            f.timelapsed = watch1.ElapsedMilliseconds;
            f1.timelapsed = watch2.ElapsedMilliseconds;
            f2.timelapsed = watch3.ElapsedMilliseconds;
            f.nonthreadtime = watch4.ElapsedMilliseconds;
            f1.nonthreadtime = watch5.ElapsedMilliseconds;
            f2.nonthreadtime = watch6.ElapsedMilliseconds;
            f.plotFourier(waveHeader.SampleRate, "Hann Window Threaded");
            f.Show();
            f1.plotFourier(waveHeader.SampleRate, "Triangle Window Threaded");
            f1.Show();
            f2.plotFourier(waveHeader.SampleRate, "DFT Threaded");
            f2.Show();
        }

        /// <summary>
        /// Creates a regular DFT based off of selection.<br/>
        /// Runs a threaded and nonthreaded DFT for benchmarking purposes.
        /// </summary>
        public void MakePlainDFT()
        {
            Stopwatch watch = new Stopwatch();
            Stopwatch watch1 = new Stopwatch();
            double[] samples = copy;
            FourierForm f = new FourierForm(this);
            f.whichfourier = 1;
            watch.Start();
            f.doFourier(samples);
            watch.Stop();
            f.whichfourier = 2;
            watch1.Start();
            f.doFourier(samples);
            watch1.Stop();
            f.timelapsed = watch.ElapsedMilliseconds;
            f.nonthreadtime = watch1.ElapsedMilliseconds;
            f.plotFourier(waveHeader.SampleRate, "DFT Threaded");
            f.Show();
        }

        /// <summary>
        /// Creates a hann DFT based off of selection.<br/>
        /// Runs a threaded and nonthreaded DFT for benchmarking purposes.
        /// </summary>
        public void MakeHann()
        {
            Stopwatch watch = new Stopwatch();
            Stopwatch watch1 = new Stopwatch();
            double[] hann = Fourier.hannWindow(copy);
            FourierForm f = new FourierForm(this);
            f.whichfourier = 1;
            watch.Start();
            f.doFourier(hann);
            watch.Stop();
            f.whichfourier = 2;
            watch1.Start();
            f.doFourier(hann);
            watch1.Stop();
            f.timelapsed = watch.ElapsedMilliseconds;
            f.nonthreadtime = watch1.ElapsedMilliseconds;
            f.plotFourier(waveHeader.SampleRate, "Hann Window Threaded");
            f.Show();
        }

        /// <summary>
        /// Creates a triangulated DFT based off of selection.<br/>
        /// Runs a threaded and nonthreaded DFT for benchmarking purposes.
        /// </summary>
        public void MakeTriangle()
        {
            Stopwatch watch = new Stopwatch();
            Stopwatch watch1 = new Stopwatch();
            double[] triangle = Fourier.triangleWindow(copy);
            FourierForm f = new FourierForm(this);
            f.whichfourier = 1;
            watch.Start();
            f.doFourier(triangle);
            watch.Stop();
            f.whichfourier = 2;
            watch1.Start();
            f.doFourier(triangle);
            watch1.Stop();
            f.timelapsed = watch.ElapsedMilliseconds;
            f.nonthreadtime = watch1.ElapsedMilliseconds;
            f.plotFourier(waveHeader.SampleRate, "Triangle Window Threaded");
            f.Show();
        }

        /// <summary>
        /// Opens a form that displays the info of the current wave.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InfoForm f = new InfoForm(filePath, waveHeader);
            f.Show();
        }

        /// <summary>
        /// Opens an OpenFileDialog to select a file to compress using differential encoding and MRLE. <br/>
        /// Will actually work on any file since header is compressed as well.<br/>
        /// *Notes for Dennis:<br/>
        /// Testing with a few friends we basically found out that MRLE kinda sucks for wav files, <br/>
        /// one of the few instances where we saw proper compression was using 28 as the key value on <br/>
        /// your one_two_three.wav file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void compressFileToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog f = new OpenFileDialog();
            byte[] file = new byte[0];
            f.Filter = "WAV File (*.wav)|*.wav|All files (*.*)|*.*";
            if (f.ShowDialog() == DialogResult.OK) {
                BinaryReader b = new BinaryReader(File.OpenRead(f.FileName));
                FileInfo fInfo = new FileInfo(f.FileName);
                Array.Resize(ref file, (int)fInfo.Length);
                file = b.ReadBytes((int)fInfo.Length);
                b.Close();
                file = Compression.DifferentialEncoding(file);
                file = Compression.MRLE(file);
                SaveFileDialog s = new SaveFileDialog();
                s.Filter = "TOTALLYNOTCOMPRESSED File (*.compressnt)|*.compressnt|All files (*.*)|*.*";
                s.DefaultExt = ".compressnt";
                if (s.ShowDialog() == DialogResult.OK) {
                    FileStream saving = (FileStream)s.OpenFile();
                    BinaryWriter bw = new BinaryWriter(saving);
                    bw.Write(file);
                    bw.Close();
                }
            }
            
        }

        /// <summary>
        /// Opens a file compressed using differential/MRLE encoding. <br/>
        /// The drawbacks of this specific version at the very least is that the key must be know to decompress. <br/>
        /// The files compressed by my MRLE are all using key 0 so when decompressing we will also be using 0 as a key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openCompressedToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "TOTALLYNOTCOMPRESSED File (*.compressnt)|*.compressnt|All files (*.*)|*.*";
            byte[] file = new byte[0];
            if (f.ShowDialog() == DialogResult.OK) {
                BinaryReader b = new BinaryReader(File.OpenRead(f.FileName));
                FileInfo fInfo = new FileInfo(f.FileName);
                Array.Resize(ref file, (int)fInfo.Length);
                file = b.ReadBytes(file.Length);
                b.Close();
                file = Compression.Decompress(file);
                file = Compression.ReverseDifferentialEncoding(file);
                SaveFileDialog s = new SaveFileDialog();
                s.Filter = "WAV File (*.wav)|*.wav|All files (*.*)|*.*";
                s.DefaultExt = ".wav";
                if (s.ShowDialog() == DialogResult.OK) {
                    FileStream saving = (FileStream)s.OpenFile();
                    BinaryWriter bw = new BinaryWriter(saving);
                    bw.Write(file);
                    bw.Close();
                }
                fileSamples = readingWave(s.FileName);
                plotFreqWaveChart(fileSamples);
            }
        }

        /// <summary>
        /// Disables/Hides buttons when form is created/cleared.
        /// </summary>
        private void buttonStyling() {
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
            somethingToolStripMenuItem.Enabled = false;
            fileDetailsToolStripMenuItem.Enabled = false;
            editToolStripMenuItem.Enabled = false;
            StartPlay.Enabled = false;
            StartPlay.Visible = false;
            TimeMS.Visible = false;
        }

        /// <summary>
        /// Enables/appears buttons when wave is plotted.
        /// </summary>
        private void enableButtons() {
            clearToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            hannWindowToolStripMenuItem1.Enabled = true;
            triangularWindowToolStripMenuItem1.Enabled = true;
            somethingToolStripMenuItem.Enabled = true;
            fileDetailsToolStripMenuItem.Enabled = true;
            editToolStripMenuItem.Enabled = true;
            hScrollBar1.Visible = true;
            StartPlay.Enabled = true;
            StartPlay.Visible = true;
            Cut.Enabled = true;
            Copy.Enabled = true;
            Cut.Visible = true;
            Copy.Visible = true;
            TimeMS.Visible = true;
        }

        /// <summary>
        /// Styling for the chart.
        /// </summary>
        private void chartStyling() {
            var ca = chart1.ChartAreas[0];
            var cs = chart1.Series["Original"];
            cs.ChartType = SeriesChartType.FastLine;
            ca.AxisX.Minimum = 0;
            ca.AxisX.Maximum = Double.NaN;
            ca.AxisX.ScrollBar.Enabled = false;
            ca.AxisX.ScaleView.Zoomable = false;
            ca.CursorX.IsUserEnabled = true;
            ca.CursorX.IsUserSelectionEnabled = true;
            cs.Color = lineColor;
            ca.CursorX.SelectionColor = selectionColor;
            cs.BorderWidth = 3;
            ca.BackColor = Color.Transparent;
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.LabelStyle.Enabled = false;
            ca.AxisY.MajorGrid.Enabled = false;
            this.chart1.MouseWheel += chart1_MouseWheel;
            hScrollBar1.Visible = false;
            ca.AxisX.ScaleView.Size = Constants.VIEWABLE;
        }

        /// <summary>
        /// So buffer can be set when we do convolution from FourierForm.cs.
        /// </summary>
        public void SetBufferInOtherWindow() {
            fileData = convertToByte(fileSamples);
            fixed (byte* ptr = fileData) {
                RecordingFunctions.setBuffer(ptr, fileData.Length, waveHeader.BitsPerSample, waveHeader.BlockAlign, (int)waveHeader.SampleRate, (int)waveHeader.ByteRate, waveHeader.NumChannels);
            }
        }

        /// <summary>
        /// Helper function to convert our samples back to bytes.<br/>
        /// Commonly used when I want to set the buffer.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Byte representation of given samples.</returns>
        public byte[] convertToByte(double[] s) {
            short[] news = new short[s.Length];
            news = s.Select(x => (short)(x)).ToArray();
            List<Byte> b = new List<byte>();
            for (int i = 0; i < news.Length; ++i) {
                b.AddRange(BitConverter.GetBytes(news[i]).ToList());
            }
            return b.ToArray();
        }

        /// <summary>
        /// Sets sample rate to 11025.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem2_Click(object sender, EventArgs e) {
            fixed (byte* ptr = fileData) {
                RecordingFunctions.setBuffer(ptr, fileData.Length, waveHeader.BitsPerSample, waveHeader.BlockAlign, 11025, (int)waveHeader.ByteRate, waveHeader.NumChannels);
            }
            waveHeader.SampleRate = 11025;
        }

        /// <summary>
        /// Sets sample rate to 22050.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem3_Click(object sender, EventArgs e) {
            fixed (byte* ptr = fileData) {
                RecordingFunctions.setBuffer(ptr, fileData.Length, waveHeader.BitsPerSample, waveHeader.BlockAlign, 22050, (int)waveHeader.ByteRate, waveHeader.NumChannels);
            }
            waveHeader.SampleRate = 22050;
        }

        /// <summary>
        /// Sets sample rate to 44100.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem4_Click(object sender, EventArgs e) {
            fixed (byte* ptr = fileData) {
                RecordingFunctions.setBuffer(ptr, fileData.Length, waveHeader.BitsPerSample, waveHeader.BlockAlign, 44100, (int)waveHeader.ByteRate, waveHeader.NumChannels);
            }
            waveHeader.SampleRate = 44100;
        }

        /// <summary>
        /// Sets sample rate to 88200.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hzToolStripMenuItem_Click(object sender, EventArgs e) {
            fixed (byte* ptr = fileData) {
                RecordingFunctions.setBuffer(ptr, fileData.Length, waveHeader.BitsPerSample, waveHeader.BlockAlign, 88200, (int)waveHeader.ByteRate, waveHeader.NumChannels);
            }
            waveHeader.SampleRate = 88200;
        }

        /// <summary>
        /// Sets bits per sample to 8. <br/>
        /// Sets block align to 1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bitsToolStripMenuItem_Click(object sender, EventArgs e) {
            fixed (byte* ptr = fileData) {
                RecordingFunctions.setBuffer(ptr, fileData.Length, 8, 1, (int)waveHeader.SampleRate, (int)waveHeader.ByteRate, waveHeader.NumChannels);
            }
            waveHeader.BitsPerSample = 8;
            waveHeader.BlockAlign = 1;
        }

        /// <summary>
        /// Sets bits per sample to 16. <br/>
        /// Sets block align to 2.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bitsToolStripMenuItem1_Click(object sender, EventArgs e) {
            fixed (byte* ptr = fileData) {
                RecordingFunctions.setBuffer(ptr, fileData.Length, 16, 2, (int)waveHeader.SampleRate, (int)waveHeader.ByteRate, waveHeader.NumChannels);
            }
            waveHeader.BitsPerSample = 16;
            waveHeader.BlockAlign = 2;
        }

        /// <summary>
        /// Sets bits per sample to 24. <br/>
        /// Sets block align to 3.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bitsToolStripMenuItem2_Click(object sender, EventArgs e) {
            fixed (byte* ptr = fileData) {
                RecordingFunctions.setBuffer(ptr, fileData.Length, 24, 3, (int)waveHeader.SampleRate, (int)waveHeader.ByteRate, waveHeader.NumChannels);
            }
            waveHeader.BitsPerSample = 24;
            waveHeader.BlockAlign = 3;
        }
    }
}
