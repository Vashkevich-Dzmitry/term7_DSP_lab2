using System;
using System.Drawing;

namespace DSP_lab2
{
    class PulseSignal : GeneratedSignal
    {
        public override SignalTypes SignalType { get; set; } = SignalTypes.Pulse;
        public override PointF[] Points { get; set; }

        public PulseSignal(float phi0, float f, int n, float a, float d): base(phi0, f, n, a)
        {
            this.d = d;
            Points = Generate(phi0, a, f, n, d);
        }

        public override PointF[] Generate(float phi0, float A, float f, int N, float? d = 0.5f)
        {
            Points = new PointF[N];
            for (int n = 0; n < N; n++)
            {
                Points[n].X = n / (float)N;
                Points[n].Y = (2 * Math.PI * f * n / N + phi0) % (2 * Math.PI) / (2 * Math.PI) <= d ? A : -A;
            }

            return Points;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
