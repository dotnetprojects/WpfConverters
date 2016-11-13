using System;
using System.Globalization;
using System.Linq;

namespace DotNetProjects.WPF.Converters
{   
    public class StringLastPartConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return ((string) value).Split(new[] {'.'}).Last();
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           throw new NotImplementedException();
        }
    }
}
