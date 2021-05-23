using System;
using System.Globalization;
using System.Windows.Data;

namespace NoiseCast.MVVM.ViewModel.Converter
{
    [ValueConversion(typeof(double), typeof(TimeSpan))]
    public class DoubleToTimeSpanConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? seconds = value as double?;

            if (seconds.HasValue)
                return TimeSpan.FromSeconds(seconds.Value);

            return TimeSpan.FromSeconds(0);
        }
    }

    [ValueConversion(typeof(string), typeof(double))]
    public class StringToTimeSpanConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? val = (double?)value;

            if (!val.HasValue || val.Value == 0) val = 0;

            return string.Format("{0:00}:{1:00}", TimeSpan.FromSeconds(val.Value).Hours, TimeSpan.FromSeconds(val.Value).Minutes);
        }
    }
}