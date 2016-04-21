using System;
using System.Globalization;
using System.Windows.Data;

namespace NotarialCompany.Common.Converters
{
    public class MultiConcatStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Join(" ", values);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("MultiConcatStringConverter is one way converter");
        }
    }
}
