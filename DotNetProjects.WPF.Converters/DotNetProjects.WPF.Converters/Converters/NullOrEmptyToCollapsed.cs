using System;
using System.Windows;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class NullOrEmptyToCollapsed : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            if (value is int)
            {
                if (((int)value) == 0)
                    return Visibility.Collapsed;

                return Visibility.Visible;
            }

            if (value is string)
            {
                if (string.IsNullOrEmpty(value as string))
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }

            return Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
