using System;

namespace DotNetProjects.WPF.Converters
{
    public class IntToBoolConverter : ValueConverter
    {
        public int Value { get; set; }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || (int)value != Value)
                return false;
            return true;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value == true)
                return Value;
            return null;
        }
    }
}