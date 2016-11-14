using System;
using System.Windows.Data;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class GuidEmptyToNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Guid.Empty;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Guid &&
                (Guid)value == Guid.Empty)
                return null;
            return value;
        }
    }
}
