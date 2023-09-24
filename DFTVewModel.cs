using ScottPlot;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;

namespace DSP_lab2
{
    class DFTVewModel : INotifyPropertyChanged
    {
        public WpfPlot AmplitudePlot { get; set; }
        public WpfPlot PhasePlot { get; set; }

        public double[]? SinSpectrum { get; private set; }
        public double[]? CosSpectrum { get; private set; }
        public double[]? AmplitudeSpectrum { get; private set; }
        public double[]? PhaseSpectrum { get; private set; }

        public ObservableCollection<Complex> ComplexValues { get; set; }

        public DFTVewModel(WpfPlot phasePlot, WpfPlot amplitudePlot)
        {
            PhasePlot = phasePlot;
            AmplitudePlot = amplitudePlot;
            ComplexValues = new ObservableCollection<Complex>();
        }

        public double[] ExecuteDFT(double[] values, int k)
        {
            SinSpectrum = ComputeSinSpectrum(values, k);
            CosSpectrum = ComputeCosSpectrum(values, k);

            AmplitudeSpectrum = ComputeAmplitudeSpectrum(k);
            PhaseSpectrum = ComputePhaseSpectrum(k);

            DisplayComplexValues(k);
            DrawCharts();

            return ComputeRestoredSignal(k);
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

        public void DisplayComplexValues(int k)
        {
            ComplexValues.Clear();

            for (int i = 0; i < k / 2; i++)
            {
                ComplexValues.Add(new Complex(CosSpectrum![i], SinSpectrum![i]));
            }
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
            double[] result = new double[k / 2];
            for (int j = 0; j < k / 2; j++)
            {
                result[j] = Math.Atan2(SinSpectrum![j], CosSpectrum![j]);
            }

            return result;
        }

        public double[] ComputeRestoredSignal(int k)
        {
            double[] values = new double[k];
            for (int i = 0; i < k; i++)
            {
                double value = 0;
                for (int j = 0; j < k / 2; j++)
                {
                    value += AmplitudeSpectrum![j] * Math.Cos(2 * Math.PI * i * j / k - PhaseSpectrum![j]);
                }

                values[i] = value;
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
