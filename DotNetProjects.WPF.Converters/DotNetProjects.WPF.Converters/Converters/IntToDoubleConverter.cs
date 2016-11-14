using System;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class IntToDoubleConverter : ValueConverter
    {
        private static readonly Lazy<ValueConverter> _instance = new Lazy<ValueConverter>(() => new IntToDoubleConverter());
        public static ValueConverter Instance { get { return _instance.Value; } }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
                return System.Convert.ToDouble(value);
            return 0.0;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}