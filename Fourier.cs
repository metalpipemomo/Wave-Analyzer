using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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
    
    public static class Fourier
    {

        //Forward Discrete Fourier Transform
        public static Complex[] DFT(double[] S, int N)
        {
            Complex[] part1 = new Complex[N];
            Thread t1 = new Thread(() =>
            {
                for (int f = 0; f < N; ++f)
                {
                    for (int t = 0; t < N; ++t)
                    {
                        part1[f].real += S[t] * Math.Cos(2 * Math.PI * t * f / N);
                    }
                    part1[f].real /= N;
                }
            });
            Thread t2 = new Thread(() =>
            {
                for (int f = 0; f < N; ++f)
                {
                    for (int t = 0; t < N; ++t)
                    {
                        part1[f].imaginary -= S[t] * Math.Sin(2 * Math.PI * t * f / N);
                    }
                    part1[f].imaginary /= N;
                }
            });
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
            return part1;
        }

        //Non-Threaded DFT
        public static Complex[] nonThreadedDFT(double[] S, int N)
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
        
        //Low-pass filter
        public static double[] lowPassFilter(int N, double fcut, double samplerate)
        {
            double[] lowpass = new double[N];
            lowpass[0] = 1;
            int bin = (int) Math.Floor((fcut * N) / samplerate);
            for (int i = 1; i < bin + 1; i++)
            {
                lowpass[i] = 1;
            }
            for (int i = N - bin; i < N; i++)
            {
                lowpass[i] = 1;
            }
            return lowpass;
        }

        //High-pass filter
        public static double[] highPassFilter(int N, double fcut, double samplerate)
        {
            double[] highpass = new double[N];
            highpass[0] = 1;
            int bin = (int) Math.Ceiling((fcut * N) / samplerate);
            for (int i = bin; i < N - bin + 1; i++)
            {
                highpass[i] = 1;
            }
            return highpass;
        }

        //Convert filter to complex array
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

        //Convolution
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
                        samples[i] += (s[j + i] * fw[j]) / fw.Length;
                        Array.Resize(ref s, m + (n - 1));
                    }
                }
            }
            return samples;
        }

        //Shannon entropy
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

        //Hann Windowing Technique
        public static double[] hannWindow(double[] samples)
        {
            int N = samples.Length;
            double[] w = new double[N];
            for (int n = 0; n < N; ++n)
            {
                w[n] = samples[n] * (1 - Math.Cos((2 * Math.PI * n) / N));
            }
            return w;
        }

        //Triangular Windowing Technique
        public static double[] triangleWindow(double[] samples)
        {
            int N = samples.Length;
            double[] w = new double[N];
            for (int n = 0; n < N; ++n)
            {
                w[n] = samples[n] * (1 - Math.Abs((n - (N / 2)) / ((N + 1) / 2)));
            }
            return w;
        }


        //Printing Samples
        public static void printSamples(double[] S)
        {
            for (int i = 0; i < S.Length; i++)
            {
                Console.WriteLine(i + ". " + S[i]);
            }
        }

        //Printing Samples using Trace
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
            double[] Amplitudes = new double[A.Length];
            for (int i = 0; i < A.Length; i++)
            {
                Amplitudes[i] = Math.Sqrt(Math.Pow(A[i].real, 2)
                    + Math.Pow(A[i].imaginary, 2)) * 2;
            }
            return Amplitudes;
        }
        //Get Frequency
        public static double[] getFrequency(Complex[] A, double samplerate)
        {
            double[] freqs = new double[A.Length];
            int N = A.Length;
            for (int f = 0; f < N; ++f)
            {
                freqs[f] = (f * samplerate) / N;
            }
            return freqs;
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

        //Used to find half of DFT chart
        public static double getLargestFreq(double[] freqs) {
            double[] copy = new double[freqs.Length];
            Array.Copy(freqs, copy, copy.Length);
            Array.Sort(copy);
            return copy[copy.Length - 1];
        }

        //Function for testing techniques learned in 3931
        public static void Testing() {
            double[] s = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            double[] fw = { 1, 0.2, 0.2, 0, 1, 0.1, 0.1, 1 };
            double[] samples = Fourier.convolve(s, fw);
            Trace.WriteLine("Convolution Test:");
            Fourier.printSamplesTrace(samples);
            Trace.WriteLine("Filter Test:");
            double srate = 1000;
            double fcut = 300;
            int size = 16;
            double[] myfilter = Fourier.lowPassFilter(size, fcut, srate);
            Fourier.printSamplesTrace(myfilter);
            myfilter = Fourier.highPassFilter(size, fcut, srate);
            Fourier.printSamplesTrace(myfilter);
            double[] shannonentropytest = { 5, 10, 15, 10, 8, 10, 15, 5, 8, 3, 8, 5, 8, 10, 10, 15, 10, 10, 8, 5, 5 };
            Trace.WriteLine("Shannon Entropy Test:");
            Trace.WriteLine(Fourier.entropy(shannonentropytest));
            Fourier.printSamplesTrace(Fourier.uniquearr(shannonentropytest));
            Fourier.printSamplesTrace(Fourier.inverseDFT(Fourier.convertFilter(myfilter), myfilter.Length));
        }
    }
}

