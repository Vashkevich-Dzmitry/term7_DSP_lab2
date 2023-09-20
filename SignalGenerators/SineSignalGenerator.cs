using System;
using System.Collections.Generic;
using System.Drawing;

namespace DSP_lab2
{
    class SineSignalGenerator : ISignalGenerator
    {
        public string SignalType { get; init; } = "Синусоидальный сигнал";

        public IList<PointF> Generate(float fi0, float A, float f, int N, float d = 0.5f)
        {
            PointF[] result = new PointF[N];

            for (int n = 0; n < N; n++)
            {
                result[n].X = n / (float)N;
                result[n].Y = (float)Math.Round(A * Math.Sin(2 * Math.PI * f * n / N + fi0), 3);
            }

            return result;
        }
    }
}
