using System;
using System.Globalization;
using System.Windows.Data;

namespace NoiseCast.MVVM.Core
{
    [ValueConversion(typeof(string), typeof(double))]
    public class OlderThanDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? val = (DateTime?)value;

            if (!val.HasValue) return "unknown";

            if (val.Value == DateTime.Today) return "Today";
            if (val.Value >= (DateTime.Now - new TimeSpan(1, 0, 0, 0))) return "Yesterday";
            if (val.Value >= (DateTime.Now - new TimeSpan(1, 0, 0, 0))) return "2 days ago";
            if (val.Value >= (DateTime.Now - new TimeSpan(1, 0, 0, 0))) return "3 days ago";
            if (val.Value >= (DateTime.Now - new TimeSpan(1, 0, 0, 0))) return "4 days ago";
            if (val.Value >= (DateTime.Now - new TimeSpan(1, 0, 0, 0))) return "5 days ago";
            if (val.Value >= (DateTime.Now - new TimeSpan(1, 0, 0, 0))) return "6 days ago";

            return val.Value.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}