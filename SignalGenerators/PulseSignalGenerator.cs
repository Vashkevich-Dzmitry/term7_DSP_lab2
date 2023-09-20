using System;
using System.Drawing;
using System.Collections.Generic;

namespace DSP_lab2
{
    class PulseSignalGenerator : ISignalGenerator
    {
        public string SignalType { get; init; } = "Прямоугольный сигнал";

        public IList<PointF> Generate(float fi0, float A, float f, int N, float d = 0.5f)
        {

            PointF[] result = new PointF[N];

            for (int n = 0; n < N; n++)
            {
                result[n].X = n / (float)N;
                result[n].Y = (2 * Math.PI * f * n / N + fi0) % (2 * Math.PI) / (2 * Math.PI) <= d ? A : -A;
            }

            return result;
        }
    }
}
