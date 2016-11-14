using System;
using System.Globalization;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class EqualityToBooleanConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return object.Equals(value, parameter);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return parameter;

            //it's false, so don't bind it back
            //throw new Exception("EqualityToBooleanConverter: It's false, I won't bind back.");
            return GetDefaultValue(targetType);
        }

        private object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }
            else
            {
                return null;
            }
        }
    }
}
