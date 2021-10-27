using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveAnalyzer
{
    public class WavReader
    {
        public int ChunkID;
        public int ChunkSize;
        public int Format;
        public int SubChunk1ID;
        public int SubChunk1Size;
        public ushort AudioFormat;
        public ushort NumChannels;
        public uint SampleRate;
        public uint ByteRate;
        public ushort BlockAlign;
        public ushort BitsPerSample;
        public int SubChunk2ID;
        public int SubChunk2Size;

        public void Empty()
        {
            ChunkID = 0;
            ChunkSize = 0;
            Format = 0;
            SubChunk1ID = 0;
            SubChunk1Size = 0;
            AudioFormat = 0;
            NumChannels = 0;
            SampleRate = 0;
            ByteRate = 0;
            BlockAlign = 0;
            BitsPerSample = 0;
            SubChunk2ID = 0;
            SubChunk2Size = 0;
        }
    }
}
