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
            //Basically the menu where you select files
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //add filter
            openFileDialog.Filter = "WAV File (*.wav)|*.wav|All files (*.*)|*.*";
            //check if file open
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            //read wave file in bytes
            byte[] buffer = File.ReadAllBytes(openFileDialog.FileName);
            
            //Grabbing Data portion of header
            short[] data = new short[buffer.Length-44];
            for (int i = 44; i < buffer.Length; i += 2)
            {
                data[i - 44] = BitConverter.ToInt16(buffer, i);
            }
            double[] theData = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                theData[i] = data[i];
            }

            //Initializing Header
            WavReader waveReader = new WavReader(
                BitConverter.ToInt32(buffer, 0),
                BitConverter.ToInt32(buffer, 4),
                BitConverter.ToInt32(buffer, 8),
                BitConverter.ToInt32(buffer, 12),
                BitConverter.ToInt32(buffer, 16),
                BitConverter.ToInt16(buffer, 20),
                BitConverter.ToInt16(buffer, 22),
                BitConverter.ToInt32(buffer, 24),
                BitConverter.ToInt32(buffer, 28),
                BitConverter.ToInt16(buffer, 32),
                BitConverter.ToInt16(buffer, 34),
                BitConverter.ToInt32(buffer, 36),
                BitConverter.ToInt32(buffer, 40),
                theData
                );
            Console.WriteLine(theData.Length);

            //monakS
            for (int i = 0; i < theData.Length; i++)
            {
                chart1.Series["Series1"].Points.AddXY(i, theData[i]);
            }
            
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void Clear_Click(object sender, EventArgs e)
        {
            chart1.Series["Series1"].Points.Clear();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            //int max = chart1.Series["Series1"].
        }
    }
}
