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

            return TimeSpan.FromSeconds(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan? time = value as TimeSpan?;

            if (time.HasValue)
                return time.Value.TotalSeconds;

            return 0;
        }
    }

    [ValueConversion(typeof(string), typeof(double))]
    public class StringToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? val = (double?)value;

            if (!val.HasValue || val.Value == 0) val = 0;

            return string.Format("{0:00}:{1:00}", TimeSpan.FromSeconds(val.Value).Hours, TimeSpan.FromSeconds(val.Value).Minutes);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}