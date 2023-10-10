using ScottPlot;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace DSP_lab2
{
    class DFTVewModel : INotifyPropertyChanged
    {
        public WpfPlot AmplitudePlot { get; set; }
        public WpfPlot PhasePlot { get; set; }

        public ObservableCollection<Complex> ComplexValues { get; set; }

        public DFTVewModel(WpfPlot phasePlot, WpfPlot amplitudePlot)
        {
            PhasePlot = phasePlot;
            AmplitudePlot = amplitudePlot;
            ComplexValues = new ObservableCollection<Complex>();
        }

        public double[] ExecuteDFT(double[] values, int k, int N)
        {
            double[] sinSpectrum = new double[k], cosSpectrum = new double[k], amplitudeSpectrum = new double[k], phaseSpectrum = new double[k];
            double[] restoredValues = new double[N];
            Complex[] complexes = new Complex[k];

            Task convertion = new(() =>
            {
                Parallel.For(0, k, (i, state) =>
                {
                    sinSpectrum[i] = 0;
                    cosSpectrum[i] = 0;
                    amplitudeSpectrum[i] = 0;
                    phaseSpectrum[i] = 0;

                    Task countComplexes = new(() =>
                    {
                        Parallel.For(0, k, (j, state) =>
                        {
                            lock (sinSpectrum) sinSpectrum[i] += values[j] * Math.Sin(2 * Math.PI * j * i / k);
                            lock (cosSpectrum) cosSpectrum[i] += values[j] * Math.Cos(2 * Math.PI * j * i / k);
                        });
                    });

                    countComplexes.Start();
                    countComplexes.Wait();

                    sinSpectrum[i] = 2 * sinSpectrum[i] / k;
                    cosSpectrum[i] = 2 * cosSpectrum[i] / k;

                    complexes[i] = new(cosSpectrum[i], sinSpectrum[i]);

                    amplitudeSpectrum[i] = Math.Sqrt(Math.Pow(sinSpectrum[i], 2) + Math.Pow(cosSpectrum[i], 2));
                    phaseSpectrum[i] = Math.Atan2(cosSpectrum[i], sinSpectrum[i]);

                });
            });

            convertion.Start();
            convertion.Wait();

            ComplexValues.Clear();
            foreach (Complex complex in complexes) ComplexValues.Add(complex);

            PhasePlot.Plot.Clear();
            PhasePlot.Plot.AddBar(phaseSpectrum, System.Drawing.Color.LightGreen);
            PhasePlot.Refresh();

            AmplitudePlot.Plot.Clear();
            AmplitudePlot.Plot.AddBar(amplitudeSpectrum, System.Drawing.Color.LightGreen);
            AmplitudePlot.Refresh();

            Task restoration = new(() =>
            {
                Parallel.For(0, N, (i, state) =>
                {
                    restoredValues[i] = amplitudeSpectrum[0] / 2;

                    Parallel.For(0, k / 2, (j, state) =>
                    {
                        lock (restoredValues) restoredValues[i] += amplitudeSpectrum[j] * Math.Sin(2 * Math.PI * i * j / N + phaseSpectrum[j]);
                    });

                });
            });

            restoration.Start();
            restoration.Wait();

            return restoredValues;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
