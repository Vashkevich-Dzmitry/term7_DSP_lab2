using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DSP_lab2
{
    class DiscreteFourierTransformation : INotifyPropertyChanged
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
            }
        }

        public Complex[] complexValues { get; private set; }
        public double[] SinSpectrum { get; private set; }
        public double[] CosSpectrum { get; private set; }
        public double[] AmplitudeSpectrum { get; private set; }
        public double[] PhaseSpectrum { get; private set; }
        public double[] RestoredSignal { get; private set; }

        public DiscreteFourierTransformation()
        {
            k = 64;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
