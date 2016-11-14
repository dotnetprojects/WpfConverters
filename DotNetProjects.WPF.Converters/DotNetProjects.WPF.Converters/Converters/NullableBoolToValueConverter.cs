using System;
using System.Windows;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class NullableBoolToValueConverter<T> : ValueConverter
    {
        public T NullValue { get; set; }
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return this.NullValue;
            else
                return ((bool?)value).Value ? this.TrueValue : this.FalseValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value.Equals(this.TrueValue))
                    return true;
                else if (value.Equals(this.FalseValue))
                    return false;
            }
            return null;
        }
    }

    public class NullableBoolToStringConverter : NullableBoolToValueConverter<String> { }
    public class NullableBoolToBoolConverter : NullableBoolToValueConverter<bool> { }
    public class NullableBoolToBrushConverter : NullableBoolToValueConverter<Brush> { }
    public class NullableBoolToVisibilityConverter : NullableBoolToValueConverter<Visibility> { }
    public class NullableBoolToObjectConverter : NullableBoolToValueConverter<Object> { }
    public class NullableBoolToThicknessConverter : NullableBoolToValueConverter<Thickness> { }
}