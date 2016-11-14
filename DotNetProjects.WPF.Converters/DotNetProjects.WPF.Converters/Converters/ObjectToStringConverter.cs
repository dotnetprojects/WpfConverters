using System;
using System.Globalization;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class ObjectToStringConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return value.ToString();
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
