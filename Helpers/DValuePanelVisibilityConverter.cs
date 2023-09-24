using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DSP_lab2.Helpers
{
    [ValueConversion(typeof(double?), typeof(Visibility))]
    public class DValuePanelVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
