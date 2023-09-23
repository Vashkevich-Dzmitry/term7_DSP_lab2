using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DSP_lab2
{
    class DFTVewModel : INotifyPropertyChanged
    {
        private const double Threshold = 0.0001;

        private int k;
        public int K
        {
            get => k;
            set
            {
                k = value;
                OnPropertyChanged(nameof(K));

                ExecuteDFT();
                DrawCharts();
            }
        }

        public Complex[] complexValues { get; private set; }
        public double[] SinSpectrum { get; private set; }
        public double[] CosSpectrum { get; private set; }
        public double[] AmplitudeSpectrum { get; private set; }
        public double[] PhaseSpectrum { get; private set; }
        public double[] RestoredSignal { get; private set; }

        public WpfPlot SignalsPlot { get; set; }
        public WpfPlot AmplitudePlot { get; set; }
        public WpfPlot PhasePlot { get; set; }

        public DFTVewModel(WpfPlot signalsPlot, WpfPlot phasePlot, WpfPlot amplitudePlot)
        {
            k = 64;

            SignalsPlot = signalsPlot;
            PhasePlot = phasePlot;
            AmplitudePlot = amplitudePlot;

            ExecuteDFT();
            DrawCharts();
        }

        public void ExecuteDFT()
        {
            SinSpectrum = ComputeSinSpectrum();
            CosSpectrum = ComputeCosSpectrum();

            AmplitudeSpectrum = ComputeAmplitudeSpectrum();
            PhaseSpectrum = ComputePhaseSpectrum();
            RestoredSignal = ComputeRestoredSignal();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
