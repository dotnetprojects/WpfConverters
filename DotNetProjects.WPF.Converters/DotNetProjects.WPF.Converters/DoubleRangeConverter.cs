using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters
{
    public class DoubleRangeValue<T>
    {
        public string DoubleRange { get; set; }
        public T Value { get; set; }
    }

    [ContentProperty("DoubleRangeValues")]
    public class DoubleRangeConverter<T> : ValueConverter
    {
        public T DefaultValue { get; set; }

        private ObservableCollection<DoubleRangeValue<T>> _doubleRangeValues = new ObservableCollection<DoubleRangeValue<T>>();
        public IList DoubleRangeValues
        {
            get { return this._doubleRangeValues; }            
        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var intvalue = System.Convert.ToDouble(value);

            foreach (DoubleRangeValue<T> intBrushValue in this.DoubleRangeValues)
            {
                if (intBrushValue.DoubleRange.Length >= 2 && intBrushValue.DoubleRange[0] == '<' && intBrushValue.DoubleRange[1] == '=')
                {
                    var val = System.Convert.ToDouble(intBrushValue.DoubleRange.Substring(2));
                    if (intvalue <= val)
                        return intBrushValue.Value;
                }
                else if (intBrushValue.DoubleRange.Length >= 2 && intBrushValue.DoubleRange[0] == '>' && intBrushValue.DoubleRange[1] == '=')
                {
                    var val = System.Convert.ToDouble(intBrushValue.DoubleRange.Substring(2));
                    if (intvalue >= val)
                        return intBrushValue.Value;
                }
                else if (intBrushValue.DoubleRange.Length >= 1 && intBrushValue.DoubleRange[0] == '<')
                {
                    var val = System.Convert.ToDouble(intBrushValue.DoubleRange.Substring(1));
                    if (intvalue < val)
                        return intBrushValue.Value;
                }
                else if (intBrushValue.DoubleRange.Length >= 1 && intBrushValue.DoubleRange[0] == '>')
                {
                    var val = System.Convert.ToDouble(intBrushValue.DoubleRange.Substring(1));
                    if (intvalue > val)
                        return intBrushValue.Value;
                }
                else if (intBrushValue.DoubleRange.Contains('-'))
                {
                    var start = System.Convert.ToDouble(intBrushValue.DoubleRange.Split('-')[0]);
                    var stop = System.Convert.ToDouble(intBrushValue.DoubleRange.Split('-')[1]);
                    if (intvalue >= start && intvalue <= stop)
                        return intBrushValue.Value;
                }
                else if (System.Convert.ToDouble(intBrushValue.DoubleRange) == intvalue)
                    return intBrushValue.Value;
            }

            return DefaultValue;          
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DoubleRangeToStringConverter : DoubleRangeConverter<string> { } ;
    public class DoubleRangeValueString : DoubleRangeValue<string> { } ;

    public class DoubleRangeToBoolConverter : DoubleRangeConverter<bool> { } ;
    public class DoubleRangeValueBool : DoubleRangeValue<bool> { } ;

    public class DoubleRangeToBrushConverter : DoubleRangeConverter<Brush> { } ;
    public class DoubleRangeValueBrush : DoubleRangeValue<Brush> { } ;

    public class DoubleRangeToColorConverter : DoubleRangeConverter<Color> { } ;
    public class DoubleRangeValueColor : DoubleRangeValue<Color> { } ;


    public class DoubleRangeToVisibilityConverter : DoubleRangeConverter<Visibility> { } ;
    public class DoubleRangeValueVisibility : DoubleRangeValue<Visibility> { } ;

    public class DoubleRangeToObjectConverter : DoubleRangeConverter<object> { } ;
    public class DoubleRangeValueObject : DoubleRangeValue<object> { } ;

    public class DoubleRangeToThicknessConverter : DoubleRangeConverter<Thickness> { } ;
    public class DoubleRangeValueThickness : DoubleRangeValue<Thickness> { } ;

    public class DoubleRangeToPointConverter : DoubleRangeConverter<Point> { } ;
    public class DoubleRangeValuePoint : DoubleRangeValue<Point> { } ;

    public class DoubleRangeToRectConverter : DoubleRangeConverter<Rect> { } ;
    public class DoubleRangeValueRect : DoubleRangeValue<Rect> { } ;

    public class DoubleRangeToHorizontalAlignmentConverter : DoubleRangeConverter<HorizontalAlignment> { } ;
    public class DoubleRangeValueHorizontalAlignment : DoubleRangeValue<HorizontalAlignment> { } ;

    public class DoubleRangeToVerticalAlignmentConverter : DoubleRangeConverter<VerticalAlignment> { } ;
    public class DoubleRangeValueVerticalAlignment : DoubleRangeValue<VerticalAlignment> { } ;


    public class DoubleRangeToByteConverter : DoubleRangeConverter<Byte> { } ;
    public class DoubleRangeValueByte : DoubleRangeValue<Byte> { } ;
    public class DoubleRangeToSByteConverter : DoubleRangeConverter<SByte> { } ;
    public class DoubleRangeValueSByte : DoubleRangeValue<SByte> { } ;
    public class DoubleRangeToUInt16Converter : DoubleRangeConverter<UInt16> { } ;
    public class DoubleRangeValueUInt16 : DoubleRangeValue<UInt16> { } ;
    public class DoubleRangeToInt16Converter : DoubleRangeConverter<Int16> { } ;
    public class DoubleRangeValueInt16 : DoubleRangeValue<Int16> { } ;
    public class DoubleRangeToUInt32Converter : DoubleRangeConverter<UInt32> { } ;
    public class DoubleRangeValueUInt32 : DoubleRangeValue<UInt32> { } ;
    public class DoubleRangeToInt32Converter : DoubleRangeConverter<Int32> { } ;
    public class DoubleRangeValueInt32 : DoubleRangeValue<Int32> { } ;
    public class DoubleRangeToUInt64Converter : DoubleRangeConverter<UInt64> { } ;
    public class DoubleRangeValueUInt64 : DoubleRangeValue<UInt64> { } ;
    public class DoubleRangeToInt64Converter : DoubleRangeConverter<Int64> { } ;
    public class DoubleRangeValueInt64 : DoubleRangeValue<Int64> { } ;
    public class DoubleRangeToSingleConverter : DoubleRangeConverter<Single> { } ;
    public class DoubleRangeValueSingle : DoubleRangeValue<Single> { } ;
    public class DoubleRangeToDoubleConverter : DoubleRangeConverter<Double> { } ;
    public class DoubleRangeValueDouble : DoubleRangeValue<Double> { } ;
    public class DoubleRangeToDecimalConverter : DoubleRangeConverter<Decimal> { } ;
    public class DoubleRangeValueDecimal : DoubleRangeValue<Decimal> { } ;    
}
