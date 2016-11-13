using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters
{

    public class IntRangeValue<T>
    {
        public string IntRange { get; set; }
        public T Value { get; set; }
    }

    public class IntRangeConverter<T> : ValueConverter
    {
        private List<IntRangeValue<T>> _intRangeValues = new List<IntRangeValue<T>>();
        public List<IntRangeValue<T>> IntRangeValues
        {
            get { return this._intRangeValues; }
            set { this._intRangeValues = value; }
        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            int intvalue = System.Convert.ToInt32(value);

            foreach (IntRangeValue<T> intBrushValue in this.IntRangeValues)
            {
                if (intBrushValue.IntRange.Contains('-'))
                {
                    int start = System.Convert.ToInt32(intBrushValue.IntRange.Split('-')[0]);
                    int stop = System.Convert.ToInt32(intBrushValue.IntRange.Split('-')[1]);
                    if (intvalue >= start && intvalue <= stop)
                        return intBrushValue.Value;
                }
                else if (intBrushValue.IntRange.Length >= 2 && intBrushValue.IntRange[0] == '<' && intBrushValue.IntRange[1] == '=')
                {
                    int val = System.Convert.ToInt32(intBrushValue.IntRange.Substring(1));
                    if (intvalue <= val)
                        return intBrushValue.Value;
                }
                else if (intBrushValue.IntRange.Length >= 2 && intBrushValue.IntRange[0] == '>' && intBrushValue.IntRange[1] == '=')
                {
                    int val = System.Convert.ToInt32(intBrushValue.IntRange.Substring(1));
                    if (intvalue >= val)
                        return intBrushValue.Value;
                }
                else if (intBrushValue.IntRange.Length >= 1 && intBrushValue.IntRange[0] == '<')
                {
                    int val = System.Convert.ToInt32(intBrushValue.IntRange.Substring(1));
                    if (intvalue < val)
                        return intBrushValue.Value;
                }
                else if (intBrushValue.IntRange.Length >= 1 && intBrushValue.IntRange[0] == '>')
                {
                    int val = System.Convert.ToInt32(intBrushValue.IntRange.Substring(1));
                    if (intvalue > val)
                        return intBrushValue.Value;
                }
                else if (System.Convert.ToInt32(intBrushValue.IntRange) == intvalue)
                    return intBrushValue.Value;
            }

            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IntRangeToStringConverter : IntRangeConverter<string> { } ;
    public class IntRangeValueString : IntRangeValue<string> { } ;

    public class IntRangeToBrushConverter : IntRangeConverter<Brush> { } ;
    public class IntRangeValueBrush : IntRangeValue<Brush> { } ;

    public class IntRangeToThicknessConverter : IntRangeConverter<Thickness> { } ;
    public class IntRangeValueThickness : IntRangeValue<Thickness> { } ;

    public class IntRangeToVisibilityConverter : IntRangeConverter<Visibility> { } ;
    public class IntRangeValueVisibility : IntRangeValue<Visibility> { } ;

    public class IntRangeToPointConverter : IntRangeConverter<Point> { } ;
    public class IntRangeValuePoint : IntRangeValue<Point> { } ;

    public class IntRangeToIntConverter : IntRangeConverter<int> { } ;
    public class IntRangeValueInt : IntRangeValue<int> { } ;

    public class IntRangeToDoubleConverter : IntRangeConverter<double> { } ;
    public class IntRangeValueDouble : IntRangeValue<double> { } ;
}
