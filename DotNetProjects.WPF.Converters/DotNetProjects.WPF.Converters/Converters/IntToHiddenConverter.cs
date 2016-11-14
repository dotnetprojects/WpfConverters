using System;
using System.Windows;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class IntToHiddenConverter : ValueConverter
    {
        private static readonly Lazy<ValueConverter> _instance = new Lazy<ValueConverter>(() => new IntToHiddenConverter());
        public static ValueConverter Instance { get { return _instance.Value; } }

        public int HiddenValue { get; set; }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (System.Convert.ToInt32(value) == this.HiddenValue)
                return Visibility.Collapsed;
            return Visibility.Visible;            
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
