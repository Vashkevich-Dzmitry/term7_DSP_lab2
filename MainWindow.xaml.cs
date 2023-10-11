using DSP_lab2.ViewModels;
using System.Windows;

namespace DSP_lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SignalViewModel signalViewModel = new(SignalsPlot, PhasePlot, AmplitudePlot);
            DataContext = signalViewModel;
        }
    }
}
