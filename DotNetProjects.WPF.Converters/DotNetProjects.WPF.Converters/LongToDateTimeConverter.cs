using System;

namespace DotNetProjects.WPF.Converters
{
    public class LongToDateTimeConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
                return new DateTime((long) value);
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
                return ((DateTime) value).Ticks;
            return null;
        }
    }

  
}