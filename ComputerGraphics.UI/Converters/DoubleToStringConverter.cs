using System;
using System.Globalization;
using System.Windows.Data;

namespace ComputerGraphics.UI.Converters
{
    internal class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return double.TryParse((string)value,out double ret) ? ret : 0;
        }
    }
}
