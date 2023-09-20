using System;
using System.Collections.Generic;
using System.Drawing;

namespace DSP_lab2
{
    class SawtoothSignalGenerator : ISignalGenerator
    {
        public string SignalType { get; init; } = "Пилообразный сигнал";
        public IList<PointF> Generate(float fi0, float A, float f, int N, float d = 0.5f)
        {
            PointF[] result = new PointF[N];

            for (int n = 0; n < N; n++)
            {
                result[n].X = n / (float)N;
                result[n].Y = (float)Math.Round((A / Math.PI) * (2 * Math.PI * f * n / N + fi0 - Math.PI + 2 * Math.PI) % (2 * Math.PI) - A, 3);
            }

            return result;
        }
    }
}
