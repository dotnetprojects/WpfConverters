using System;
using System.Windows;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class StringEmptyOrEqualVisibillityConverter : ValueConverter
    {
        public Visibility FalseValue { get; set; }
        public Visibility TrueValue { get; set; }

        public StringEmptyOrEqualVisibillityConverter()
        {
            FalseValue = Visibility.Visible;
            TrueValue = Visibility.Collapsed;
        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var valueString = value as string;
            if (valueString == null)
                return TrueValue;

            if (string.IsNullOrWhiteSpace(valueString))
                return TrueValue;

            var parameterString = parameter as string;

            return valueString == parameterString ? TrueValue : FalseValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
