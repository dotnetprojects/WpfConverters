﻿using System;
using System.Windows;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters
{
    public class BoolToValueConverter<T> : ValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return FalseValue;
            else
                return System.Convert.ToBoolean(value) ? TrueValue : FalseValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? value.Equals(TrueValue) : false;
        }
    }
    
    public class BoolToVisibilityConverter : BoolToValueConverter<Visibility>
    {
        public BoolToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Hidden;
        }
    }

    public class BoolToStringConverter : BoolToValueConverter<String> { }
    public class BoolToBoolConverter : BoolToValueConverter<bool> { }
    public class BoolToBrushConverter : BoolToValueConverter<Brush> { }
    public class BoolToObjectConverter : BoolToValueConverter<Object> { }
    public class BoolToThicknessConverter : BoolToValueConverter<Thickness> { }
}