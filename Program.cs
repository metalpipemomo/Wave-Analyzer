using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveAnalyzer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            double[] s = { 1, 3, 2, 6, 8, 4, 2, 3, 1, 1, 3, 5};
            double[] fw = { 1.1, 2.1, 0.3, 0.2 };
            double[] samples = Fourier.convolve(s, fw);
        }
    }
}
