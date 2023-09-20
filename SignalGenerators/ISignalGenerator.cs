using System.Collections.Generic;
using System.Drawing;

namespace DSP_lab2
{
    interface ISignalGenerator
    {
        public string SignalType { get; init; }
        public IList<PointF> Generate(float fi0, float A, float f, int N, float d = 0.5f);
    }
}
