using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPFMVVM.MVVM.Core
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class VisabilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = (bool)value;

            if (isVisible)
                return Visibility.Visible;

            return Visibility.Hidden;
        }
    }
}