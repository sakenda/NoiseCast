using System;
using System.Globalization;
using System.Windows.Data;

namespace NoiseCast.MVVM.Core
{
    [ValueConversion(typeof(double), typeof(TimeSpan))]
    public class DoubleToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? seconds = value as double?;

            if (seconds.HasValue)
                return TimeSpan.FromSeconds(seconds.Value);

            return TimeSpan.Zero;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan? time = value as TimeSpan?;

            if (time.HasValue)
                return time.Value.TotalSeconds;

            return 0;
        }
    }
}