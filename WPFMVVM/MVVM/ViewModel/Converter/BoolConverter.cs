using System;
using System.Globalization;
using System.Windows.Data;

namespace NoiseCast.MVVM.ViewModel.Converter
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class IsSubscribedBoolToStringConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? val = (bool?)value;

            if (!val.HasValue) return "Subscribe";

            return val.Value == true ? "Unsubscribe" : "Subscribe";
        }
    }

    [ValueConversion(typeof(bool), typeof(string))]
    public class IsArchivedBoolToStringConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? val = (bool?)value;

            if (!val.HasValue) return "unknown";

            return val.Value == true ? "heard" : "not finished";
        }
    }
}