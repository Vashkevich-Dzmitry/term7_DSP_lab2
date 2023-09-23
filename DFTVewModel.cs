using ScottPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DSP_lab2
{
    class DFTVewModel : INotifyPropertyChanged
    {
        //private const double Threshold = 0.0001;

        public WpfPlot AmplitudePlot { get; set; }
        public WpfPlot PhasePlot { get; set; }

        public ObservableCollection<double>? SinSpectrum { get; private set; }
        public ObservableCollection<double>? CosSpectrum { get; private set; }
        public ObservableCollection<double>? AmplitudeSpectrum { get; private set; }
        public ObservableCollection<double>? PhaseSpectrum { get; private set; }

        public DFTVewModel(WpfPlot phasePlot, WpfPlot amplitudePlot)
        {
            PhasePlot = phasePlot;
            AmplitudePlot = amplitudePlot;
        }

        public double[] ExecuteDFT(double[] values, int N, int k)
        {
            SinSpectrum = new(ComputeSinSpectrum(values, k));
            CosSpectrum = new(ComputeCosSpectrum(values, k));

            AmplitudeSpectrum = new(ComputeAmplitudeSpectrum(k));
            PhaseSpectrum = new(ComputePhaseSpectrum(k));

            DrawCharts();

            return ComputeRestoredSignal(N, k);
        }

        public void DrawCharts()
        {
            PhasePlot.Plot.Clear();
            PhasePlot.Plot.AddBar(PhaseSpectrum!.ToArray(), System.Drawing.Color.LightGreen);
            PhasePlot.Refresh();

            AmplitudePlot.Plot.Clear();
            AmplitudePlot.Plot.AddBar(AmplitudeSpectrum!.ToArray(), System.Drawing.Color.LightGreen);
            AmplitudePlot.Refresh();
        }

        public static double[] ComputeSinSpectrum(double[] values, int k) //Im
        {
            double[] result = new double[k / 2];
            for (int j = 0; j < k / 2; j++)
            {
                double value = 0;
                for (int i = 0; i < k; i++)
                {
                    value += values[i] * Math.Sin(2 * Math.PI * i * j / k);
                }

                result[j] = 2 * value / k;
            }

            return result;
        }

        public static double[] ComputeCosSpectrum(double[] values, int k) //Re
        {
            double[] result = new double[k / 2];
            for (int j = 0; j < k / 2; j++)
            {
                double value = 0;
                for (int i = 0; i < k; i++)
                {
                    value += values[i] * Math.Cos(2 * Math.PI * i * j / k);
                }

                result[j] = 2 * value / k;
            }

            return result;
        }

        public double[] ComputeAmplitudeSpectrum(int k)
        {
            double[] result = new double[k / 2];
            for (int j = 0; j < k / 2; j++)
            {
                result[j] = Math.Sqrt(Math.Pow(SinSpectrum![j], 2) + Math.Pow(CosSpectrum![j], 2));
            }

            return result;
        }

        public double[] ComputePhaseSpectrum(int k)
        {
            double[] values = new double[k / 2];
            for (int j = 0; j < k / 2; j++)
            {
                values[j] = Math.Atan2(SinSpectrum![j], CosSpectrum![j]);
            }

            return values;
        }

        public double[] ComputeRestoredSignal(int N, int k)
        {
            double[] values = new double[k];
            for (int i = 0; i < k; i++)
            {
                double val = 0;
                for (int j = 1; j < k / 2; j++)
                {
                    val += AmplitudeSpectrum![j] * Math.Cos(2 * Math.PI * i * j / k - PhaseSpectrum![j]);
                }

                val += AmplitudeSpectrum![0] / 2;
                values[i] = val;
            }

            return values;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
