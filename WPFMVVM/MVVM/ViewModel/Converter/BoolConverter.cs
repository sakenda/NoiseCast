using System;
using System.Globalization;
using System.Windows.Data;

namespace NoiseCast.MVVM.ViewModel.Converter
{
    [ValueConversion(typeof(string), typeof(double))]
    public class BoolToStringConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? val = (bool?)value;

            if (!val.HasValue) return "Subscribe";

            return val.Value == true ? "Unsubscribe" : "Subscribe";
        }
    }
}