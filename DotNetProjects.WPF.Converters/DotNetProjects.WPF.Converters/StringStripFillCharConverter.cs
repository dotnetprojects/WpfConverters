using System;
using System.Windows.Data;

namespace DotNetProjects.WPF.Converters
{
    public class StringStripFillCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return "";
            return value.ToString().Replace(".", "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
