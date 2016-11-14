using System;
using System.Collections;
using System.Linq;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class IEnumerableToCountConverter : ValueConverter
    {
        private static readonly Lazy<ValueConverter> _instance = new Lazy<ValueConverter>(() => new IEnumerableToCountConverter());
        public static ValueConverter Instance { get { return _instance.Value; } }


        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return 0;

            return ((IEnumerable)value).Cast<object>().Count();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
