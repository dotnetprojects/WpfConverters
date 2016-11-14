using System;
using System.Globalization;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class aaDoubleMultiplicationConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            double intvalue = System.Convert.ToDouble(value);

            double intmulti = System.Convert.ToDouble(parameter);


            return intvalue*intmulti;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }  
}
