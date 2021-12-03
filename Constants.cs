using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveAnalyzer
{
    class Constants
    {
        /// <summary>
        /// A lot of these wav header variables are constants because when writing<br/>
        /// we don't have to check for garbage values and we know what values we are <br/>
        /// recording at by virtue of having access to the DLL source files.
        /// </summary>

        //Used for when there are garbage values in header.
        public const int MAXHEADERSIZE = 500;
        //How many samples you can see per scroll.
        public const int VIEWABLE = 5000;
        //Value of RIFF used for writing.
        public const int RIFF = 1179011410;
        //Value of WAVE used for writing.
        public const int WAVE = 1163280727;
        //Value of fmt used for writing.
        public const int fmt = 544501094;
        //Value of Data used for writing.
        public const int data = 1635017060;
        //Value of PCM used for writing.
        public const ushort PCM = 1;
        //Value of Sample rate used for writing.
        public const uint SAMPLE_RATE = 44100;
        //Value of Byte rate used for writing.
        public const uint BYTE_RATE = 88200;
        //Value of Chunk size used for writing.
        public const int CHUNK_SIZE = 36;
        //Value of Block align used for writing.
        public const ushort BLOCK_ALIGN = 2;
        //Value of Bits per sample used for writing.
        public const ushort BITS_PER_SAMPLE = 16;
        //Value of Subchunk1size used for writing.
        public const int SUBCHUNK_1SIZE = 16;
        //Value of Channels used for writing.
        public const ushort CHANNELS = 1;
    }
}
