using System;
using System.Windows;
using System.Windows.Data;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class GuidEmptyToCollapsedConverter : IValueConverter
    {
        private object CurValue;
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            CurValue = value;
            if (value is Guid &&
                ((Guid)value) != Guid.Empty)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return CurValue;
        }
    }
}
