using System;
using System.Windows.Data;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters
{
    public class IntIndexToBrushConverter : IValueConverter
    {
        public Brush DefaultValue { get; set; }
        public Brush Value0 { get; set; }
        public Brush Value1 { get; set; }
        public Brush Value2 { get; set; }
        public Brush Value3 { get; set; }
        public Brush Value4 { get; set; }
        public Brush Value5 { get; set; }
        public Brush Value6 { get; set; }
        public Brush Value7 { get; set; }
        public Brush Value8 { get; set; }
        public Brush Value9 { get; set; }
        public Brush Value10 { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var wr = System.Convert.ToInt32(value);
            switch (wr)
            {
                case 0: if (Value0 == null) break; return Value0;
                case 1: if (Value1 == null) break; return Value1;
                case 2: if (Value2 == null) break; return Value2;
                case 3: if (Value3 == null) break; return Value3;
                case 4: if (Value4 == null) break; return Value4;
                case 5: if (Value5 == null) break; return Value5;
                case 6: if (Value6 == null) break; return Value6;
                case 7: if (Value7 == null) break; return Value7;
                case 8: if (Value8 == null) break; return Value8;
                case 9: if (Value9 == null) break; return Value9;
                case 10: if (Value10 == null) break; return Value10;
            }

            if (DefaultValue != null)
                return DefaultValue;
            return Value0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                if ((Brush)value == Value1)
                    return 1;
                if ((Brush)value == Value2)
                    return 2;
                if ((Brush)value == Value3)
                    return 3;
                if ((Brush)value == Value4)
                    return 4;
            }

            return 0;
        }
    }
}

