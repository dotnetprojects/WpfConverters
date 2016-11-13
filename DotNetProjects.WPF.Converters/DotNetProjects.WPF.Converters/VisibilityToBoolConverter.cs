using System;
using System.Windows;

namespace DotNetProjects.WPF.Converters
{
    public class VisibilityToBoolConverter : ValueConverter
    {
        public bool VisibleValue { get; set; }
        public bool CollapsedValue { get; set; }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return this.CollapsedValue;
            else
                return ((Visibility)value) == Visibility.Visible ? this.VisibleValue : this.CollapsedValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(this.VisibleValue) ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public class VisibleToTrueConverter : VisibilityToBoolConverter
    {
        public VisibleToTrueConverter()
        {
            CollapsedValue = false;
            VisibleValue = true;
        }
    }
}