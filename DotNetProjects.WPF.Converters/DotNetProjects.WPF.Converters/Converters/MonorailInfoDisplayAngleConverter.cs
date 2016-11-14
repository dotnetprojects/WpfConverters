using System;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class MonorailInfoDisplayAngleConverter: ValueConverter
    {        
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 0.0 - (double)value;            
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
