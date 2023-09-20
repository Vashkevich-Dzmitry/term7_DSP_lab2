using ScottPlot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DSP_lab2
{
    class SignalWorker
    {
        private readonly List<GeneratedSignal> GeneratedSignals;

        public SignalWorker()
        {
            GeneratedSignals = new();
        }

        public static void DrawChart(PointF[] points, WpfPlot plot)
        {
            plot.Plot.Clear();
            plot.Plot.AddScatter(points.Select(p => (double)p.X).ToArray(), points.Select(p => (double)p.Y).ToArray(), Color.LightGreen, 7);
            plot.Refresh();
        }

        public void RemoveSignalAt(int index)
        {
            GeneratedSignals.RemoveAt(index);
        }

        public List<GeneratedSignal> GetSignals()
        {
            return GeneratedSignals;
        }

        public void AddSignal(GeneratedSignal signal, DataGrid grid)
        {
            GeneratedSignals.Add(signal);
            grid.Items.Refresh();
        }
    }
}
