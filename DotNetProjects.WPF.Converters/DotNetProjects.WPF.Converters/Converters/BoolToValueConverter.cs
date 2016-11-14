using System;
using System.Windows;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class BoolToValueConverter<T> : ValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return this.FalseValue;
            else
                return (bool)value ? this.TrueValue : this.FalseValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? value.Equals(this.TrueValue) : false;
        }
    }

    public class BoolToStringConverter : BoolToValueConverter<String> { }

    public class BoolToBoolConverter : BoolToValueConverter<bool>
    {
        private static readonly Lazy<ValueConverter> _instance = new Lazy<ValueConverter>(() => new BoolToBoolConverter());
        public static ValueConverter Instance { get { return _instance.Value; } }

        public BoolToBoolConverter()
        {
            TrueValue = false;
            FalseValue = true;
        }
    }

    public class EqualsToBoolConverter : BoolToValueConverter<String>
    {
        public string CompareValue { get; set; }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return this.FalseValue;
            else if (CompareValue != null)
                return value.ToString() == CompareValue ? this.TrueValue : this.FalseValue;
            return System.Convert.ToBoolean(value) ? this.TrueValue : this.FalseValue;
        }
    }

    public class EqualsToVisibilityConverter : BoolToValueConverter<Visibility>
    {
        public string CompareValue { get; set; }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return this.FalseValue;
            else if (CompareValue != null)
                return value.ToString() == CompareValue ? this.TrueValue : this.FalseValue;
            return System.Convert.ToBoolean(value) ? this.TrueValue : this.FalseValue;
        }
    }

    public class EqualsToBrushConverter : BoolToValueConverter<Brush>
    {
        public string CompareValue { get; set; }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return this.FalseValue;
            else if (CompareValue != null)
                return String.Equals(value.ToString(), CompareValue, StringComparison.CurrentCultureIgnoreCase) ? this.TrueValue : this.FalseValue;
            return System.Convert.ToBoolean(value) ? this.TrueValue : this.FalseValue;
        }
    }

    public class BoolToBrushConverter : BoolToValueConverter<Brush> { }
    
    public class BoolToColorConverter : BoolToValueConverter<Color> { }

    public class BoolToVisibilityConverter : BoolToValueConverter<Visibility>
    {
        private static readonly Lazy<ValueConverter> _trueIsVisibleConverter = new Lazy<ValueConverter>(() => new BoolToVisibilityConverter() {TrueValue = Visibility.Visible, FalseValue = Visibility.Collapsed});
        public static ValueConverter TrueIsVisibleConverter { get { return _trueIsVisibleConverter.Value; } }
    }
    public class BoolToObjectConverter : BoolToValueConverter<Object> { }
    public class BoolToThicknessConverter : BoolToValueConverter<Thickness> { }
    public class BoolToPointConverter : BoolToValueConverter<Point> { }
    public class BoolToRectConverter : BoolToValueConverter<Rect> { }
    public class BoolToHorizontalAlignmentConverter : BoolToValueConverter<HorizontalAlignment> { }
    public class BoolToVerticalAlignmentConverter : BoolToValueConverter<VerticalAlignment> { }
    public class BoolToStyleConverter : BoolToValueConverter<Style> { }

    public class BoolToByteConverter : BoolToValueConverter<Byte> { }
    public class BoolToSByteConverter : BoolToValueConverter<SByte> { }
    public class BoolToUInt16Converter : BoolToValueConverter<UInt16> { }
    public class BoolToInt16Converter : BoolToValueConverter<Int16> { }
    public class BoolToUInt32Converter : BoolToValueConverter<UInt32> { }
    public class BoolToInt32Converter : BoolToValueConverter<Int32> { }
    public class BoolToUInt64Converter : BoolToValueConverter<UInt64> { }
    public class BoolToInt64Converter : BoolToValueConverter<Int64> { }
    public class BoolToSingleConverter : BoolToValueConverter<Single> { }
    public class BoolToDoubleConverter : BoolToValueConverter<Double> { }
    public class BoolToDecimalConverter : BoolToValueConverter<Decimal> { }
}