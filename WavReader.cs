using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveAnalyzer
{
    //For reading??
    /*public struct wavFilehdr
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
    };*/
    public class WavReader
    {
        //RIFF
        public int ChunkID;
        public int ChunkSize;
        public int Format;
        public int SubChunk1ID;
        public int SubChunk1Size;
        public short AudioFormat;
        public short NumChannels;
        public int SampleRate;
        public int ByteRate;
        public short BlockAlign;
        public short BitsPerSample;
        public int SubChunk2ID;
        public int SubChunk2Size;
        public double[] Data;

        public WavReader(int ChunkID, int ChunkSize, int Format, int SubChunk1ID,
            int SubChunk1Size, short AudioFormat, short NumChannels, int SampleRate,
            int ByteRate, short BlockAlign, short BitsPerSample, int SubChunk2ID,
            int SubChunk2Size, double[] Data)
        {
            this.ChunkID = ChunkID;
            this.ChunkSize = ChunkSize;
            this.Format = Format;
            this.SubChunk1ID = SubChunk1ID;
            this.SubChunk1Size = SubChunk1Size;
            this.AudioFormat = AudioFormat;
            this.NumChannels = NumChannels;
            this.SampleRate = SampleRate;
            this.ByteRate = ByteRate;
            this.BlockAlign = BlockAlign;
            this.BitsPerSample = BitsPerSample;
            this.SubChunk2ID = SubChunk2ID;
            this.SubChunk2Size = SubChunk2Size;
            this.Data = Data;
        }

        public double[] getData()
        {
            return Data;
        }
        /*//For writing??
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

        }*/

    }
}
