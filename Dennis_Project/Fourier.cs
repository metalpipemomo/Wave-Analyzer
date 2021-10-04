using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        //Printing Samples
        public static void printSamples(double[] S)
        {
            for (int i = 0; i < S.Length; i++)
            {
                Console.WriteLine(i + ". " + Math.Round(S[i], 5));
            }
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

