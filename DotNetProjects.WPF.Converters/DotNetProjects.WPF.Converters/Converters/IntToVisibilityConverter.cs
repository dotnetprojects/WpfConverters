using System;
using System.Windows;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class IntToVisibilityConverter : ValueConverter
    {
        public int VisibleValue { get; set; }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (System.Convert.ToInt32(value) != this.VisibleValue)
                return Visibility.Collapsed;
            return Visibility.Visible;            
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
