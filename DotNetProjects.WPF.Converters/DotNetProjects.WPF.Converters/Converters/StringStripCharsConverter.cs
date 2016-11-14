using System;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class StringStripCharsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return "";

            var rgx = new Regex("[a-zA-Z_]");

            return rgx.Replace(value.ToString(), "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
