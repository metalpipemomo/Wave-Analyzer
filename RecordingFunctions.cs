using System;
using System.Runtime.InteropServices;

namespace WaveAnalyzer {
    unsafe class RecordingFunctions {
        /// <summary>
        /// DLL functions.
        /// </summary>
        [DllImport("RecordingDLL.dll")] public static extern int start();
        [DllImport("RecordingDLL.dll")] public static extern IntPtr getBuffer();
        [DllImport("RecordingDLL.dll", CallingConvention = CallingConvention.Cdecl)] public static extern void setBuffer(byte* newpbuffer, int length, int bps, int blockalign, int samplerate, int byterate, int nchannels);
        [DllImport("RecordingDLL.dll")] public static extern int getDwLength();
        [DllImport("RecordingDLL.dll")] public static extern IntPtr StartMessage();
        [DllImport("RecordingDLL.dll")] public static extern IntPtr StopMessage();
        [DllImport("RecordingDLL.dll")] public static extern IntPtr PlayMessage();
        [DllImport("RecordingDLL.dll")] public static extern IntPtr PlayStopMessage();
    }
}
