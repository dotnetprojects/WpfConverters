using System;

namespace DotNetProjects.WPF.Converters
{
    public class DoubleToLog10Converter : ValueConverter
    {
        #region IValueConverter Members

        public override object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double val = (double) value;
            return Math.Log10(val);
        }

        public override object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double val = (double) value;
            return Math.Pow(10, val);
        }

        #endregion
    }
}
