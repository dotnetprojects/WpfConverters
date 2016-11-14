using System;
using System.Globalization;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class EnumToTranslateStringConverter : ValueConverter
    {
        private static readonly Lazy<ValueConverter> _instance = new Lazy<ValueConverter>(() => new EnumToTranslateStringConverter());
        public static ValueConverter Instance { get { return _instance.Value; } }
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            return "ENUM_" + value.GetType().Name + "_" + value.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
