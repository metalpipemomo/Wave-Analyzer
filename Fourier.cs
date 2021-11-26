using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WaveAnalyzer
{
    //Structure to hold real and imaginary numbers
    public struct Complex
    {
        //Instance Variables
        public double real;
        public double imaginary;
        //Struct Constructor
        public Complex(double real, double imaginary)
        {
            this.real = real;
            this.imaginary = imaginary;
        }
    }
    //Structure used to generate samples
    public struct CosWave
    {
        //Instance Variables
        public double amplitude;
        public double frequency;
        public double phaseShift;
        //Struct Constructor
        public CosWave(double amplitude, double frequency, double phaseShift)
        {
            this.amplitude = amplitude;
            this.frequency = frequency;
            this.phaseShift = phaseShift;
        }
        //Calculate function
        public double calculate(int t, int N)
        {
            return amplitude * Math.Cos(frequency * 2 * Math.PI * t / N * phaseShift);
        }
    }
    public static class Fourier
    {
        //Forward Discrete Fourier Transform
        public static Complex[] DFT(double[] S, int N)
        {
            Complex[] A = new Complex[N];
            for (int f = 0; f < N; f++)
            {
                for (int t = 0; t < N; t++)
                {
                    A[f].real += S[t] * Math.Cos(2 * Math.PI * t * f / N);
                    A[f].imaginary -= S[t] * Math.Sin(2 * Math.PI * t * f / N);
                }
                A[f].real /= N;
                A[f].imaginary /= N;
            }
            return A;
        }
        //public static Complex[] FastForwardFourier(double[])
        //Inverse Discrete Fourier Transform
        public static double[] inverseDFT(Complex[] A, int N)
        {
            double[] S = new double[N];
            for (int t = 0; t < N; t++)
            {
                for (int f = 0; f < N; f++)
                {
                    S[t] += A[f].real * Math.Cos(2 * Math.PI * t * f / N)
                        - A[f].imaginary * Math.Sin(2 * Math.PI * t * f / N);
                }
            }
            return S;
        }
        //Filter frequencies out
        
        //low-pass filter
        public static double[] lowPassFilter(int N, double fcut, double samplerate)
        {
            double[] lowpass = new double[N];
            lowpass[0] = 1;
            int bin = (int) Math.Floor((fcut * N) / samplerate);
            for (int i = 1; i < bin + 1; i++)
            {
                lowpass[i] = 1;
            }
            for (int i = bin + 1; i < N - bin; i++)
            {
                lowpass[i] = 0;
            }
            for (int i = N - bin; i < N; i++)
            {
                lowpass[i] = 1;
            }
            return lowpass;
        }

        //high-pass filter
        public static double[] highPassFilter(int N, double fcut, double samplerate)
        {
            double[] highpass = new double[N];
            highpass[0] = 1;
            int bin = (int) Math.Ceiling((fcut * N) / samplerate);
            for (int i = 1; i < bin + 1; i++)
            {
                highpass[i] = 0;
            }
            for (int i = bin + 1; i < N - bin; i++)
            {
                highpass[i] = 1;
            }
            for (int i = N - bin; i < N; i++)
            {
                highpass[i] = 0;
            }
            return highpass;
        }

        //convert filter to complex arr
        public static Complex[] convertFilter(double[] samples)
        {
            Complex[] arr = new Complex[samples.Length];
            for (int i = 0; i < samples.Length; i++)
            {
                arr[i].real = samples[i];
                arr[i].imaginary = 0;
            }
            return arr;
        }

        //convolution
        public static double[] convolve(double[] s, double[] fw)
        {
            double[] samples = new double[s.Length];
            int m = s.Length;
            int n = fw.Length;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((j + i) <= m)
                    {
                        samples[i] += s[j + i] * fw[j];
                        Array.Resize(ref s, m + (n - 1));
                    }
                }
            }
            return samples;
        }

        //huffman
        public static double entropy(double[] samples)
        {
            double size = (double)samples.Length;
            double entropy = 0;
            double[] uniquearray = uniquearr(samples);
            for (int i = 0; i < uniquearray.Length; i++)
            {
                entropy += (findoccurance(samples, uniquearray[i])/size) * Math.Log(size/(findoccurance(samples, uniquearray[i])), 2);
            }
            return entropy;
        }

        //entropy helper
        public static double findoccurance(double[] samples, double num)
        {
            int occurs = 0;
            for (int i = 0; i < samples.Length; i++)
            {
                if (samples[i] == num)
                {
                    occurs++;
                }
            }
            return (double) occurs;
        }

        //entropy helper
        public static double[] uniquearr(double[] samples)
        {
            List<double> uniquearr = new List<double>();
            for (int i = 0; i < samples.Length; i++)
            {
                if (!uniquearr.Contains(samples[i]))
                {
                    uniquearr.Add(samples[i]);
                }
            }
            return uniquearr.ToArray();
        }

        //
        public static double[] triangleWindow(double[] samples)
        {
            int N = samples.Length;
            double[] w = new double[N];
            for (int n = 0; n < N; ++n)
            {
                w[n] = 1 - Math.Abs((n - (N / 2)) / ((N + 1) / 2));
            }
            return w;
        }

        public static double[] hannWindow(double[] samples)
        {
            int N = samples.Length;
            double[] w = new double[N];
            for (int n = 0; n < N; ++n)
            {
                w[n] = 0.5 * (1 - Math.Cos((2 * Math.PI * n) / N));
            }
            return w;
        }

        //Printing Samples
        public static void printSamples(double[] S)
        {
            for (int i = 0; i < S.Length; i++)
            {
                Console.WriteLine(i + ". " + Math.Round(S[i], 5));
            }
        }
        public static void printSamplesTrace(double[] S)
        {
            string s = "[ ";
            for (int i = 0; i < S.Length; i++)
            {
                s += S[i] + " ";
            }
            s += "]";
            Trace.WriteLine(s);
        }
        //Printing Complex Numbers
        public static void printComplex(Complex[] A)
        {
            for (int i = 0; i < A.Length; i++)
            {
                Console.WriteLine(i + ". (" + Math.Round(A[i].real, 5)
                    + ", " + Math.Round(A[i].imaginary, 5) + ")");
            }
        }
        //Get Amplitudes
        public static double[] getAmplitudes(Complex[] A)
        {
            double[] Amplitudes = new double[A.Length / 2 + 1];
            for (int i = 0; i < A.Length / 2 + 1; i++)
            {
                Amplitudes[i] = Math.Sqrt(Math.Pow(A[i].real, 2)
                    + Math.Pow(A[i].imaginary, 2)) * 2;
            }
            return Amplitudes;
        }
        //Generate Samples
        public static double[] getSamples(CosWave[] wave, int N)
        {
            double[] S = new double[N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < wave.Length; j++)
                {
                    S[i] += wave[i].calculate(i, N);
                }
            }
            return S;
        }
        //Convert Radians to Degrees
        public static double toDegrees(double radian)
        {
            return radian * 180 / Math.PI;
        }
        //Get Phase Shifts
        public static double[] getShifts(Complex[] A)
        {
            double[] phaseShifts = new double[A.Length];
            for (int i = 0; i < A.Length; i++)
            {
                phaseShifts[i] = toDegrees(Math.Atan2(A[i].imaginary, A[i].real));
            }
            return phaseShifts;
        }
    }
}

