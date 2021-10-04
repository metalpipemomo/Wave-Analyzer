using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WaveAnalyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //add filter
            openFileDialog.Filter = "WAV File (*.wav)|*.wav|All files (*.*)|*.*";
            //check if file open
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            //read wave file in bytes (bitconverter.toint
            byte[] buffer = File.ReadAllBytes(openFileDialog.FileName);
            
            //Grabbing Data portion of header
            short[] data = new short[buffer.Length-44];
            int index = 0;
            for (int i = 44; i < buffer.Length; i += 2)
            {
                data[index] = BitConverter.ToInt16(buffer, i);
            }
            double[] theData = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                theData[i] = data[i];
            }

            //Initializing Header
            WavReader waveReader = new WavReader(
                BitConverter.ToInt16(buffer, 0),
                BitConverter.ToInt16(buffer, 4),
                BitConverter.ToInt16(buffer, 8),
                BitConverter.ToInt16(buffer, 12),
                BitConverter.ToInt16(buffer, 16),
                BitConverter.ToInt16(buffer, 20),
                BitConverter.ToInt16(buffer, 22),
                BitConverter.ToInt16(buffer, 24),
                BitConverter.ToInt16(buffer, 28),
                BitConverter.ToInt16(buffer, 32),
                BitConverter.ToInt16(buffer, 34),
                BitConverter.ToInt16(buffer, 36),
                BitConverter.ToInt16(buffer, 40),
                theData
                );

            //x is buckets, y is ampl
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
