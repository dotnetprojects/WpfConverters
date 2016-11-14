using System;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class NullToBrushConverter : ValueConverter
    {
        public Brush NullBrush { get; set; }
        public Brush NotNullBrush { get; set; }

        public NullToBrushConverter()
        {
            NullBrush = new SolidColorBrush(Colors.White);
            NotNullBrush = new SolidColorBrush(Colors.Black);
        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return NullBrush;
            else
                return NotNullBrush;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
