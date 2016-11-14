using System;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class IntToBoolConverter : ValueConverter
    {
        private static readonly Lazy<ValueConverter> _instance = new Lazy<ValueConverter>(() => new IntToBoolConverter());
        public static ValueConverter Instance { get { return _instance.Value; } }

        public int Value { get; set; }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || (int)value != this.Value)
                return false;
            return true;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value == true)
                return this.Value;
            return null;
        }
    }
}