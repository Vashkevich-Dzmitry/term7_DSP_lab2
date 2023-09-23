using System.ComponentModel;
using System.Drawing;

namespace DSP_lab2.Signals
{
    public abstract class GeneratedSignal : INotifyPropertyChanged
    {
        protected float phi0;
        protected float f;
        protected int n;
        protected float a;
        protected float? d;

        public float Phi0
        {
            get => phi0;
            set
            {
                phi0 = value;
                Generate(phi0, a, f, n, d);
                OnPropertyChanged(nameof(Phi0));
            }
        }
        public float F
        {
            get => f;
            set
            {
                f = value;
                Generate(phi0, a, f, n, d);
                OnPropertyChanged(nameof(F));
            }
        }
        public int N
        {
            get => n;
            set
            {
                n = value;
                Generate(phi0, a, f, n, d);
                //OnPropertyChanged(nameof(N));
            }
        }
        public float A
        {
            get => a;
            set
            {
                a = value;
                Generate(phi0, a, f, n, d);
                OnPropertyChanged(nameof(A));
            }
        }

        public float? D
        {
            get => d;
            set
            {
                d = value;
                Generate(phi0, a, f, n, d);
                OnPropertyChanged(nameof(A));
            }
        }

        public abstract SignalTypes SignalType { get; set; }
        public abstract PointF[] Points { get; set; }

        public GeneratedSignal(float phi0, float f, int n, float a)
        {
            this.phi0 = phi0;
            this.f = f;
            this.n = n;
            this.a = a;
        }

        public abstract PointF[] Generate(float Phi0, float A, float f, int N, float? d = null);

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
