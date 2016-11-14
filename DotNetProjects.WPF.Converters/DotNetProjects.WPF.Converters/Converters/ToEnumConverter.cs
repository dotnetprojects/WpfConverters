using System;
using System.Collections.Generic;
using System.Globalization;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class ToEnumConverter : ValueConverter
    {
        private static readonly Lazy<ValueConverter> _instance = new Lazy<ValueConverter>(() => new ToEnumConverter());
        public static ValueConverter Instance { get { return _instance.Value; } }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return Enum.ToObject(targetType, System.Convert.ToInt32(value));

            return null;
        }
        
        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is KeyValuePair<int, string>)
                value = ((KeyValuePair<int, string>) value).Key;
            return Enum.Parse(targetType, Enum.GetName(targetType, value), false);
        }
    }
}
