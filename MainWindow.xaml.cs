using DSP_lab2.Signals;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DSP_lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SignalViewModel signalViewModel;
        public MainWindow()
        {
            InitializeComponent();
            signalViewModel = new(SignalsPlot);

            DataContext = signalViewModel;

        }

        private void SignalTypeComboBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SignalTypeComboBox.SelectedValue is { })
            {
                switch ((SignalTypes)SignalTypeComboBox.SelectedValue) {
                    case SignalTypes.Sine:
                        DutyCyclePanel.Visibility = Visibility.Collapsed;
                        SignalsGrid.SelectedItem = new SineSignal((float)Phi0Value.Value, (float)FValue.Value, (int)NValue.Value, (float)AValue.Value);
                        break;
                    case SignalTypes.Triangle:
                        DutyCyclePanel.Visibility = Visibility.Collapsed;
                        SignalsGrid.SelectedItem = new TriangleSignal((float)Phi0Value.Value, (float)FValue.Value, (int)NValue.Value, (float)AValue.Value);
                        break;
                    case SignalTypes.Sawtooth:
                        DutyCyclePanel.Visibility = Visibility.Collapsed; 
                        SignalsGrid.SelectedItem = new SawtoothSignal((float)Phi0Value.Value, (float)FValue.Value, (int)NValue.Value, (float)AValue.Value);
                        break;
                    case SignalTypes.Pulse:
                        DutyCyclePanel.Visibility = Visibility.Visible;
                        SignalsGrid.SelectedItem = new PulseSignal((float)Phi0Value.Value, (float)FValue.Value, (int)NValue.Value, (float)AValue.Value, (float)DValue.Value);
                        break;
                }

                //пересчитать всё
            }
        }

        private void DValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //пересчитать всё
        }

        private void NValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //пересчитать всё
        }

        private void FValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //пересчитать всё
        }

        private void AValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //пересчитать всё
        }

        private void Phi0Value_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //fi0 = e.NewValue * Math.PI;
            //пересчитать всё
        }

        private void SignalsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SignalsGrid.SelectedItem != null)
            {
                //DisplaySignalParamsPanel(SignalsGrid.SelectedIndex);
            }
        }

        private void DisplaySignalParamsPanel(int index)
        {
            throw new NotImplementedException();
        }
    }
}
