using System;
using System.Linq;
using System.Windows;

namespace DotNetProjects.WPF.Converters
{
    public class EnumEqualToVisibilityConverter : ValueConverter
    {
        public Visibility FalseValue { get; set; }
        public Visibility TrueValue { get; set; }
        public Visibility DefaultValue { get; set; }

        public EnumEqualToVisibilityConverter()
        {
            FalseValue = Visibility.Collapsed;
            TrueValue = Visibility.Visible;
            DefaultValue = Visibility.Collapsed;
        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return DefaultValue;

            if (!parameter.ToString().Contains("|"))
                return value.ToString() == parameter.ToString() ? this.TrueValue : this.FalseValue;
            else
            {
                var tokens = parameter.ToString().Split('|');

                if (tokens.Any(token => value.ToString() == token))
                {
                    return TrueValue;
                }

                return FalseValue;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
