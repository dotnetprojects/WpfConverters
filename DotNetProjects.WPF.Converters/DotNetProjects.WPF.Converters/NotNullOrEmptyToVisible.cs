using System;
using System.Windows;

namespace DotNetProjects.WPF.Converters
{
    public class NotNullOrEmptyToVisible : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null &&
                value is string &&
                !string.IsNullOrEmpty(((string)value)))
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
