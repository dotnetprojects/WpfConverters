using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class IntRangeValue<T>
    {
        public string IntRange { get; set; }
        public T Value { get; set; }
    }

    [ContentProperty("IntRangeValues")]
    public class IntRangeConverter<T> : ValueConverter
    {
        public T DefaultValue { get; set; }

        private ObservableCollection<IntRangeValue<T>> _intRangeValues = new ObservableCollection<IntRangeValue<T>>();
        public IList IntRangeValues
        {
            get { return this._intRangeValues; }
            //set { this._intRangeValues = value; }
        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            int intvalue = System.Convert.ToInt32(value);

            foreach (IntRangeValue<T> intBrushValue in this.IntRangeValues)
            {
                if (intBrushValue.IntRange.Length>=2 && intBrushValue.IntRange[0] == '<' && intBrushValue.IntRange[1] == '=')
                {
                    int val = System.Convert.ToInt32(intBrushValue.IntRange.Substring(2));
                    if (intvalue <= val)
                        return intBrushValue.Value;
                }
                else if (intBrushValue.IntRange.Length >= 2 && intBrushValue.IntRange[0] == '>' && intBrushValue.IntRange[1] == '=')
                {
                    int val = System.Convert.ToInt32(intBrushValue.IntRange.Substring(2));
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
                else if (intBrushValue.IntRange.Contains('-'))
                {
                    int start = System.Convert.ToInt32(intBrushValue.IntRange.Split('-')[0]);
                    int stop = System.Convert.ToInt32(intBrushValue.IntRange.Split('-')[1]);
                    if (intvalue >= start && intvalue <= stop)
                        return intBrushValue.Value;
                }
                else if (System.Convert.ToInt32(intBrushValue.IntRange) == intvalue)
                    return intBrushValue.Value;
            }

            return DefaultValue;          
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IntRangeToStringConverter : IntRangeConverter<string> { } ;
    public class IntRangeValueString : IntRangeValue<string> { } ;

    public class IntRangeToBoolConverter : IntRangeConverter<bool> { } ;
    public class IntRangeValueBool : IntRangeValue<bool> { } ;

    public class IntRangeToBrushConverter : IntRangeConverter<Brush> { } ;
    public class IntRangeValueBrush : IntRangeValue<Brush> { } ;

    public class IntRangeToColorConverter : IntRangeConverter<Color> { } ;
    public class IntRangeValueColor : IntRangeValue<Color> { } ;


    public class IntRangeToVisibilityConverter : IntRangeConverter<Visibility> { } ;
    public class IntRangeValueVisibility : IntRangeValue<Visibility> { } ;

    public class IntRangeToObjectConverter : IntRangeConverter<object> { } ;
    public class IntRangeValueObject : IntRangeValue<object> { } ;

    public class IntRangeToThicknessConverter : IntRangeConverter<Thickness> { } ;
    public class IntRangeValueThickness : IntRangeValue<Thickness> { } ;
    
    public class IntRangeToPointConverter : IntRangeConverter<Point> { } ;
    public class IntRangeValuePoint : IntRangeValue<Point> { } ;

    public class IntRangeToRectConverter : IntRangeConverter<Rect> { } ;
    public class IntRangeValueRect : IntRangeValue<Rect> { } ;

    public class IntRangeToHorizontalAlignmentConverter : IntRangeConverter<HorizontalAlignment> { } ;
    public class IntRangeValueHorizontalAlignment : IntRangeValue<HorizontalAlignment> { } ;

    public class IntRangeToVerticalAlignmentConverter : IntRangeConverter<VerticalAlignment> { } ;
    public class IntRangeValueVerticalAlignment : IntRangeValue<VerticalAlignment> { } ;


    public class IntRangeToByteConverter : IntRangeConverter<Byte> { } ;
    public class IntRangeValueByte : IntRangeValue<Byte> { } ;
    public class IntRangeToSByteConverter : IntRangeConverter<SByte> { } ;
    public class IntRangeValueSByte : IntRangeValue<SByte> { } ;
    public class IntRangeToUInt16Converter : IntRangeConverter<UInt16> { } ;
    public class IntRangeValueUInt16 : IntRangeValue<UInt16> { } ;
    public class IntRangeToInt16Converter : IntRangeConverter<Int16> { } ;
    public class IntRangeValueInt16 : IntRangeValue<Int16> { } ;
    public class IntRangeToUInt32Converter : IntRangeConverter<UInt32> { } ;
    public class IntRangeValueUInt32 : IntRangeValue<UInt32> { } ;
    public class IntRangeToInt32Converter : IntRangeConverter<Int32> { } ;
    public class IntRangeValueInt32 : IntRangeValue<Int32> { } ;
    public class IntRangeToUInt64Converter : IntRangeConverter<UInt64> { } ;
    public class IntRangeValueUInt64 : IntRangeValue<UInt64> { } ;
    public class IntRangeToInt64Converter : IntRangeConverter<Int64> { } ;
    public class IntRangeValueInt64 : IntRangeValue<Int64> { } ;
    public class IntRangeToSingleConverter : IntRangeConverter<Single> { } ;
    public class IntRangeValueSingle : IntRangeValue<Single> { } ;
    public class IntRangeToDoubleConverter : IntRangeConverter<Double> { } ;
    public class IntRangeValueDouble : IntRangeValue<Double> { } ;
    public class IntRangeToDecimalConverter : IntRangeConverter<Decimal> { } ;
    public class IntRangeValueDecimal : IntRangeValue<Decimal> { } ;    
}
