using System;
using System.Windows;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class NullToVisibleConverter : ValueConverter
    {
        private static readonly Lazy<ValueConverter> _instance = new Lazy<ValueConverter>(() => new NullToVisibleConverter());
        public static ValueConverter Instance { get { return _instance.Value; } }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}