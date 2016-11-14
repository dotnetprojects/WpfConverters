using System;
using System.Collections;
using System.Linq;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class IEnumerableConverter : ValueConverter
    {
        private static readonly Lazy<ValueConverter> _instance = new Lazy<ValueConverter>(() => new IEnumerableConverter());
        public static ValueConverter Instance { get { return _instance.Value; } }


        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string converted = "";

            var list = value as IEnumerable;
            if (list != null)
            {
                converted = string.Join(";", list.Cast<object>());
            }


            return converted;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
