using System;
using System.Globalization;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class IntMultiplicationConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            int intvalue = System.Convert.ToInt32(value);

            int intmulti = System.Convert.ToInt32(parameter);


            return intvalue*intmulti;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }  
}
