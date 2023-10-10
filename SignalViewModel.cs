﻿using DSP_lab2.Helpers;
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
                if (value >= k)
                {
                    n = value;
                    OnPropertyChanged(nameof(N));

                    (ResultingX, ResultingY) = ComputeResultingSignal(N);
                }
            }
        }

        private int k;
        public int K
        {
            get => k;
            set
            {
                if (value <= N)
                {
                    k = value;
                    OnPropertyChanged(nameof(K));

                    RestoredY = new(DFT.ExecuteDFT(GetKElementsFromN(ResultingY), K, N));
                }
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
                        selectedSignal.PropertyChanged -= (object? sender, PropertyChangedEventArgs args) =>
                        {
                            (ResultingX, ResultingY) = ComputeResultingSignal(N);
                        };
                    }

                    selectedSignal = value;
                    IsSignalSelected = selectedSignal != null;

                    if (selectedSignal != null)
                    {
                        selectedSignal.PropertyChanged += (object? sender, PropertyChangedEventArgs args) =>
                        {
                            (ResultingX, ResultingY) = ComputeResultingSignal(N);
                        };
                    }

                    OnPropertyChanged(nameof(SelectedSignal));
                }
            }
        }

        private bool isComplexVisible;
        public bool IsComplexVisible
        {
            get => isComplexVisible;
            set
            {
                if (isComplexVisible != value)
                {
                    isComplexVisible = value;
                    OnPropertyChanged(nameof(IsComplexVisible));
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
                    OnPropertyChanged(nameof(IsSignalSelected));
                }
            }
        }

        public ObservableCollection<GeneratedSignal> Signals { get; set; }

        private ObservableCollection<double> resultingY;
        public ObservableCollection<double> ResultingY
        {
            get => resultingY;
            set
            {
                resultingY = value;
                OnPropertyChanged(nameof(ResultingY));

                RestoredY = new(DFT.ExecuteDFT(GetKElementsFromN(ResultingY), K, N));
                DrawCharts();
            }
        }


        private ObservableCollection<double> resultingX;
        public ObservableCollection<double> ResultingX
        {
            get => resultingX;
            set
            {
                resultingX = value;
                OnPropertyChanged(nameof(ResultingX));
            }
        }

        private ObservableCollection<double> restoredY;
        public ObservableCollection<double> RestoredY
        {
            get => restoredY;
            set
            {
                restoredY = value;
                OnPropertyChanged(nameof(RestoredY));
            }
        }

        public (ObservableCollection<double> x, ObservableCollection<double> y) ComputeResultingSignal(int pointsAmount)
        {
            ConcurrentBag<(double x, double y)> concurrentResult = new();

            Parallel.ForEach(Signals.SelectMany(signal => signal.Generate(signal.Phi0, signal.A, signal.F, pointsAmount, signal.D)).GroupBy(point => point.X),
                group =>
                {
                    concurrentResult.Add(((double)group.Key, (double)group.Sum(point => point.Y)));
                });

            return (new ObservableCollection<double>(concurrentResult.OrderBy(item => item.x).Select(item => item.x)),
                new ObservableCollection<double>(concurrentResult.OrderBy(item => item.x).Select(item => item.y)));
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
                    GeneratedSignal signal = new SineSignal(0, 1, 1);
                    Signals.Insert(0, signal);
                    SelectedSignal = signal;

                    (_, ResultingY) = ComputeResultingSignal(N);
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

                        (_, ResultingY) = ComputeResultingSignal(N);
                    }
                }, (obj) => SelectedSignal != null && Signals.Count > 1);
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
                            SignalTypes.Triangle => new TriangleSignal(SelectedSignal!.Phi0, SelectedSignal!.F, SelectedSignal!.A),
                            SignalTypes.Sawtooth => new SawtoothSignal(SelectedSignal!.Phi0, SelectedSignal!.F, SelectedSignal!.A),
                            SignalTypes.Pulse => new PulseSignal(SelectedSignal!.Phi0, SelectedSignal!.F, SelectedSignal!.A, 0.5f),
                            SignalTypes.Sine => new SineSignal(SelectedSignal!.Phi0, SelectedSignal!.F, SelectedSignal!.A),
                            _ => new CosineSignal(SelectedSignal!.Phi0, SelectedSignal!.F, SelectedSignal!.A),
                        };

                        int signalIndex = Signals.IndexOf(SelectedSignal);
                        Signals.Insert(signalIndex, newSignal);
                        SelectedSignal = Signals[signalIndex];
                        Signals.RemoveAt(signalIndex + 1);

                        (_, ResultingY) = ComputeResultingSignal(N);
                    }
                }, (obj) => SelectedSignal != null && obj != null);
            }
        }

        public DFTVewModel DFT { get; set; }

        public SignalViewModel(WpfPlot signalsPlot, WpfPlot phasePlot, WpfPlot amplitudePlot)
        {
            isComplexVisible = false;
            n = 128;
            k = 64;

            Signals = new ObservableCollection<GeneratedSignal>
            {
                new SineSignal(0, 1, 1)
            };

            SignalsPlot = signalsPlot;
            PhasePlot = phasePlot;
            AmplitudePlot = amplitudePlot;

            (resultingX, resultingY) = ComputeResultingSignal(N);

            DFT = new DFTVewModel(phasePlot, amplitudePlot);
            restoredY = new(DFT.ExecuteDFT(GetKElementsFromN(ResultingY), K, N));

            DrawCharts();
        }

        private void DrawCharts()
        {
            SignalsPlot.Plot.Clear();
            SignalsPlot.Plot.SetAxisLimits(xMin: 0, xMax: 1);
            SignalsPlot.Plot.AddScatter(ResultingX.ToArray(), ResultingY.ToArray(), System.Drawing.Color.LightGreen, 11);
            SignalsPlot.Plot.AddScatter(ResultingX.ToArray(), RestoredY.ToArray(), System.Drawing.Color.Red, 3);
            SignalsPlot.Refresh();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private double[] GetKElementsFromN(ObservableCollection<double> doubles)
        {
            (_, ObservableCollection < double> y) = ComputeResultingSignal(K);
            return y.ToArray();
        }
    }
}
