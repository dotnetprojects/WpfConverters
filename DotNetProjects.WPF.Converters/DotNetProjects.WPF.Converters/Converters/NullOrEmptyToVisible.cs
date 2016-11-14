using System;
using System.Windows;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class NullOrEmptyToVisible : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Visible;

            if (value is int)
            {
                if (((int)value) == 0)
                    return Visibility.Visible;

                return Visibility.Collapsed;
            }

            if (value is string)
            {
                if (string.IsNullOrEmpty(value as string))
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
