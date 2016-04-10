using System;
using System.Globalization;
using System.Windows.Data;

namespace NotarialCompany.Common.Converters
{
    public class NullToTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("NullToTrueConverter is one way converter");
        }
    }
}
