using System;
using System.Globalization;
using System.Windows.Data;

namespace NoiseCast.MVVM.ViewModel.Converter
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class OlderThanDateConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? val = (DateTime?)value;

            if (!val.HasValue) return "unknown";

            if (val.Value == DateTime.Today) return "Today";
            if (val.Value >= (DateTime.Now - new TimeSpan(1, 0, 0, 0))) return "Yesterday";
            if (val.Value >= (DateTime.Now - new TimeSpan(2, 0, 0, 0))) return "2 days ago";
            if (val.Value >= (DateTime.Now - new TimeSpan(3, 0, 0, 0))) return "3 days ago";

            return val.Value.ToShortDateString();
        }
    }
}