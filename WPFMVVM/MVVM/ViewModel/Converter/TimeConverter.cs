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

            if (!val.HasValue || val.Value == 0) return "unplayed";
            if (val.Value == -1) return "finished";

            if (TimeSpan.FromSeconds(val.Value) > TimeSpan.FromHours(1))
                return string.Format("{0:00}:{1:00} hours left", TimeSpan.FromSeconds(val.Value).Hours, TimeSpan.FromSeconds(val.Value).Minutes);

            return string.Format("{0:00} minutes left", TimeSpan.FromSeconds(val.Value).TotalMinutes);
        }
    }
}