using System;
using System.Globalization;
using System.Windows.Data;

namespace NoiseCast.MVVM.ViewModel.Converter
{
    [ValueConversion(typeof(double), typeof(string))]
    public class DoubleToPercentValueStringConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? number = value as double?;

            if (number.HasValue)
                return number.Value.ToString("P0");

            return null;
        }
    }
}