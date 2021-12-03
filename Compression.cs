using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveAnalyzer {
    static class Compression {
        //The key used for MRLE compression.
        private static readonly byte KEY = 0;
        //Since bytes only go up to 255, we need to compress a maximum of 255 per run.
        private static readonly byte LIMIT = 255;

        /// <summary>
        /// Differential Encodes an array of bytes.
        /// </summary>
        /// <param name="b">Array of bytes.</param>
        /// <returns>Differential encoding of given array.</returns>
        public static byte[] DifferentialEncoding(byte[] b) {
            List<Byte> diff = new List<Byte>();
            int n = b.Length;
            diff.Add(b[0]);
            for (int i = 1; i < n; ++i) {
                diff.Add((byte)(b[i - 1] - b[i]));
            }
            return diff.ToArray();
        }

        /// <summary>
        /// Compresses a byte array using Modified Run Length Encoding and a Key.
        /// </summary>
        /// <param name="b">Array of bytes.</param>
        /// <returns>MRLE compression of given array.</returns>
        public static byte[] MRLE(byte[] b) {
            List<Byte> compressed = new List<Byte>();
            int n = b.Length;
            for (int i = 0; i < n; ++i) {
                int count = 1;
                while (i < n - 1 && b[i] == b[i + 1] && count < LIMIT) {
                    ++count;
                    ++i;
                }
                if (count > 2 || b[i] == KEY) {
                    compressed.Add(KEY);
                    compressed.Add((byte)count);
                    compressed.Add(b[i]);
                } else {
                    for (int j = 0; j < count; ++j) {
                        compressed.Add(b[i]);
                    }
                }
            }
            return compressed.ToArray();
        }

        /// <summary>
        /// Decompresses a byte array that was compressing using MRLE using the same key it was compressed with.
        /// </summary>
        /// <param name="b"></param>
        /// <returns>Decompressed array of bytes.</returns>
        public static byte[] Decompress(byte[] b) {
            List<Byte> decompress = new List<Byte>();
            int n = b.Length;
            for (int i = 0; i < n; ++i) {
                if (b[i] == KEY) {
                    int size = b[i + 1];
                    for (int j = 0; j < size; ++j) {
                        decompress.Add(b[i + 2]);
                    }
                    i += 2;
                } else {
                    decompress.Add(b[i]);
                }
            }
            return decompress.ToArray();
        }

        /// <summary>
        /// Reversing the differential encoding done before compression.
        /// </summary>
        /// <param name="b"></param>
        /// <returns>Reversed differential encoding of array of bytes.</returns>
        public static byte[] ReverseDifferentialEncoding(byte[] b) {
            List<Byte> rediff = new List<Byte>();
            int n = b.Length;
            rediff.Add(b[0]);
            for (int i = 1; i < n - 1; ++i) {
                rediff.Add((byte)(rediff[i - 1] - b[i]));
            }
            return rediff.ToArray();
        }
    }
}
