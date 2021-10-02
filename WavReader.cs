using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveAnalyzer
{
    
    public struct wavFilehdr
    {
        public int RIFF;
        public int filesize_minus_4;
        public int WAVE;
        public int fmt_;
        public int fmt_size;
        public short format_tag;
        public short nchannels;
        public int samples_per_sec;
        public int avg_bytes_per_sec;
        public short nblock_align;
        public short bits_per_sample;
        public int data;
        public int data_size;

        public wavFilehdr(int RIFF, int filesize_minus_4, int WAVE, int fmt_, int fmt_size,
            short format_tag, short nchannels, int samples_per_sec, int avg_bytes_per_sec,
            short nblock_align, short bits_per_sample, int data, int data_size)
        {
            this.RIFF = RIFF;
            this.filesize_minus_4 = filesize_minus_4;
            this.WAVE = WAVE;
            this.fmt_ = fmt_;
            this.fmt_size = fmt_size;
            this.format_tag = format_tag;
            this.nchannels = nchannels;
            this.samples_per_sec = samples_per_sec;
            this.avg_bytes_per_sec = avg_bytes_per_sec;
            this.nblock_align = nblock_align;
            this.bits_per_sample = bits_per_sample;
            this.data = data;
            this.data_size = data_size;
        }
    };
    public static class WavReader
    {
        public static void WriteWav()
        {
            uint sampleNums = 44100;
            ushort channelNums = 1;
            ushort sampleLength = 1;
            uint sampleRate = 22050;
            FileStream file = new FileStream("sample.wav", FileMode.Create);
            BinaryWriter bw = new BinaryWriter(file);
            bw.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
            bw.Write(36 + sampleNums * channelNums * sampleLength);
            bw.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt"));
            bw.Write(16);
            bw.Write((ushort)1);
            bw.Write(channelNums);
            bw.Write(sampleRate);
            bw.Write(sampleRate * sampleLength * channelNums);
            bw.Write(sampleLength * channelNums);
            bw.Write((ushort)(8 * sampleLength));
            bw.Write(System.Text.Encoding.ASCII.GetBytes("data"));

        }
    }
}
