using System;
using System.Drawing;

namespace DSP_lab2.Signals
{
    class SawtoothSignal : GeneratedSignal
    {
        public override string SignalName { get; set; }
        public override PointF[] Points { get; set; }

        public SawtoothSignal(float phi0, float f, int n, float a) : base(phi0, f, n, a)
        {
            SignalName = "sawtooth";
            Points = Generate(phi0, a, f, n);
        }

        public override PointF[] Generate(float phi0, float A, float f, int N, float d = 0.5f)
        {
            Points = new PointF[N];
            for (int n = 0; n < N; n++)
            {
                Points[n].X = n / (float)N;
                Points[n].Y = (float)Math.Round((A / Math.PI) * (2 * Math.PI * f * n / N + phi0 - Math.PI + 2 * Math.PI) % (2 * Math.PI) - A, 3);
            }

            return Points;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
