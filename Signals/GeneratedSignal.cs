using System.ComponentModel;
using System.Drawing;

namespace DSP_lab2
{
    public abstract class GeneratedSignal : INotifyPropertyChanged
    {
        protected float phi0;
        protected float f;
        protected int n;
        protected float a;

        public float Phi0
        {
            get => phi0;
            set
            {
                phi0 = value;
                Generate(phi0, a, f, n);
                OnPropertyChanged(nameof(Phi0));
            }
        }
        public float F
        {
            get => f;
            set
            {
                f = value;
                Generate(phi0, a, f, n);
                OnPropertyChanged(nameof(F));
            }
        }
        public int N
        {
            get => n;
            set
            {
                n = value;
                Generate(phi0, a, f, n);
                OnPropertyChanged(nameof(N));
            }
        }
        public float A
        {
            get => a;
            set
            {
                a = value;
                Generate(phi0, a, f, n);
                OnPropertyChanged(nameof(A));
            }
        }

        public abstract string SignalName { get; set; }
        public abstract PointF[] Points { get; set; }

        public GeneratedSignal(float phi0, float f, int n, float a)
        {
            this.phi0 = phi0;
            this.f = f;
            this.n = n;
            this.a = a;
        }

        public abstract PointF[] Generate(float fi0, float A, float f, int N, float d = 0.5f);

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
