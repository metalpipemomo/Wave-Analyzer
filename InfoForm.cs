using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveAnalyzer
{
    public partial class InfoForm : Form
    {
        public InfoForm(string windowname, WavReader hdr)
        {
            InitializeComponent();
            Text = "Information";
            label1.Text = windowname;
            label2.Text = "Length: " + (hdr.SubChunk2Size / hdr.SampleRate).ToString() + " s";
            label3.Text = "Sample Rate: " + hdr.SampleRate.ToString() + " Hz";
            label4.Text = "Bits Per Sample: " + hdr.BitsPerSample.ToString();
            label5.Text = "Channels: " + hdr.NumChannels.ToString();
            label6.Text = "Size: " + hdr.ChunkSize.ToString() + " bytes";
        }
    }
}
