using System;
using System.Windows;

namespace DotNetProjects.WPF.Converters
{
    public class NullToCollapsedConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}