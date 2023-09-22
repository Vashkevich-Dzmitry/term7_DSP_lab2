using DSP_lab2.Helpers;
using DSP_lab2.Signals;
using ScottPlot;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DSP_lab2
{
    class SignalViewModel : INotifyPropertyChanged
    {
        private int n;
        public int N
        {
            get => n;
            set
            {
                n = value;
                RegenerateSignals();
                OnPropertyChanged(nameof(N));
                ResultingSignal = ComputeResultingSignal();
            }
        }

        private void RegenerateSignals()
        {
            foreach (var signal in Signals)
            {
                signal.N = N;
            }
        }

        private GeneratedSignal? selectedSignal;
        public GeneratedSignal? SelectedSignal
        {
            get { return selectedSignal; }
            set
            {
                if (selectedSignal != value)
                {
                    if (selectedSignal != null)
                    {
                        selectedSignal.PropertyChanged -= (object? sender, PropertyChangedEventArgs args) => { ResultingSignal = ComputeResultingSignal(); };
                    }

                    selectedSignal = value;
                    IsSignalSelected = selectedSignal != null;

                    if (selectedSignal != null)
                    {
                        selectedSignal.PropertyChanged += (object? sender, PropertyChangedEventArgs args) => { ResultingSignal = ComputeResultingSignal(); };
                    }
                    
                    OnPropertyChanged(nameof(SelectedSignal));
                }
            }
        }

        private bool isSignalSelected;
        public bool IsSignalSelected
        {
            get => isSignalSelected;
            set
            {
                if (isSignalSelected != value)
                {
                    isSignalSelected = value;
                    OnPropertyChanged(nameof(isSignalSelected));
                }
            }
        }

        public ObservableCollection<GeneratedSignal> Signals { get; set; }

        private ObservableCollection<(double x, double y)> resultingSignal;
        public ObservableCollection<(double x, double y)> ResultingSignal
        {
            get => resultingSignal;
            set
            {
                resultingSignal = value;
                OnPropertyChanged(nameof(resultingSignal));

                DrawCharts();
            }
        }

        public ObservableCollection<(double x, double y)> ComputeResultingSignal()
        {
            ConcurrentBag<(double x, double y)> concurrentResult = new();

            Parallel.ForEach(Signals.SelectMany(signal => signal.Points).GroupBy(point => point.X),
                group =>
                {
                    concurrentResult.Add(((double)group.Key, (double)group.Sum(point => point.Y)));
                });

            return new ObservableCollection<(double x, double y)>(concurrentResult.OrderBy(item => item.x));
        }

        public WpfPlot SignalsPlot { get; set; }
        public WpfPlot AmplitudePlot { get; set; }
        public WpfPlot PhasePlot { get; set; }


        private RelayCommand? addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??= new RelayCommand(obj =>
                {
                    GeneratedSignal signal = new SineSignal(0, 1, N, 1);
                    Signals.Insert(0, signal);
                    SelectedSignal = signal;

                    ResultingSignal = ComputeResultingSignal();
                }, (obj) => Signals.Count < 5);
            }
        }
        private RelayCommand? deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??= new RelayCommand(obj =>
                {
                    if (obj is GeneratedSignal signal)
                    {
                        Signals.Remove(signal);
                        SelectedSignal = null;

                        ResultingSignal = ComputeResultingSignal();
                    }
                }, (obj) => SelectedSignal != null);
            }
        }

        private RelayCommand? changeTypeCommand;
        public RelayCommand ChangeTypeCommand
        {
            get
            {
                return changeTypeCommand ??= new RelayCommand(obj =>
                {
                    SignalTypes? signalType = obj as SignalTypes?;
                    if (signalType != null && signalType == SelectedSignal!.SignalType)
                    {
                        GeneratedSignal newSignal = signalType switch
                        {
                            SignalTypes.Triangle => new TriangleSignal(SelectedSignal!.Phi0, SelectedSignal!.F, N, SelectedSignal!.A),
                            SignalTypes.Sawtooth => new SawtoothSignal(SelectedSignal!.Phi0, SelectedSignal!.F, N, SelectedSignal!.A),
                            SignalTypes.Pulse => new PulseSignal(SelectedSignal!.Phi0, SelectedSignal!.F, N, SelectedSignal!.A, 0.5f),
                            _ => new SineSignal(SelectedSignal!.Phi0, SelectedSignal!.F, N, SelectedSignal!.A),
                        };

                        int signalIndex = Signals.IndexOf(SelectedSignal);
                        Signals.Insert(signalIndex, newSignal);
                        SelectedSignal = Signals[signalIndex];
                        Signals.RemoveAt(signalIndex + 1);

                        ResultingSignal = ComputeResultingSignal();
                    }
                }, (obj) => SelectedSignal != null && obj != null);
            }
        }

        public DiscreteFourierTransformation DFT { get; set; }

        public SignalViewModel(WpfPlot signalsPlot, WpfPlot phasePlot, WpfPlot amplitudePlot)
        {
            n = 128;

            Signals = new ObservableCollection<GeneratedSignal>
            {
                new SineSignal(0, 1, N, 1),
                new SineSignal(0, 1, N, 1),
                new SineSignal(0, 1, N, 1),
            };

            DFT = new DiscreteFourierTransformation();

            SignalsPlot = signalsPlot;
            PhasePlot = phasePlot;
            AmplitudePlot = amplitudePlot;

            resultingSignal = ComputeResultingSignal();
            DrawCharts();
        }

        private void DrawCharts() //переделать на рисование всех графиков по очереди, вынести рисование каждого в отдельную
        {
            SignalsPlot.Plot.Clear();
            SignalsPlot.Plot.SetAxisLimits(xMin: 0, xMax: 1);
            SignalsPlot.Plot.AddScatter(ResultingSignal.Select(point => point.x).ToArray(), ResultingSignal.Select(point => point.y).ToArray(), System.Drawing.Color.LightGreen, 5);
            SignalsPlot.Refresh();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
