﻿using System;
using System.Drawing;

namespace DSP_lab2.Signals
{
    class TriangleSignal : GeneratedSignal
    {
        public override SignalTypes SignalType { get; set; } = SignalTypes.Triangle;
        public override PointF[] Points { get; set; }

        public TriangleSignal(float phi0, float f, int n, float a) : base(phi0, f, n, a)
        {
            Points = Generate(phi0, a, f, n);
        }

        public override PointF[] Generate(float phi0, float A, float f, int N, float? d = null)
        {
            Points = new PointF[N];
            for (int n = 0; n < N; n++)
            {
                Points[n].X = n / (float)N;
                Points[n].Y = (float)Math.Round((2 * A / Math.PI) * Math.Abs(Math.Abs((((2 * Math.PI * f * n) / N + phi0 - (Math.PI / 2)) % (2 * Math.PI))) - Math.PI) - A, 3);
            }

            return Points;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
