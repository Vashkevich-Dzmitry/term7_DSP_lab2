using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace DSP_lab2
{
    class NullToEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}