using System;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class IntToBrushConverter : ValueConverter
    {
        private static readonly Lazy<ValueConverter> _instance = new Lazy<ValueConverter>(() => new IntToBrushConverter());
        public static ValueConverter Instance { get { return _instance.Value; } }


        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var bytes = BitConverter.GetBytes((int)value);
            return new SolidColorBrush(Color.FromArgb(bytes[0], bytes[1], bytes[2], bytes[3]));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}