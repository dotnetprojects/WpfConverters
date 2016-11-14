using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class ListStringConverter : ValueConverter
    {
        private static readonly Lazy<ValueConverter> _instance = new Lazy<ValueConverter>(() => new ListStringConverter());
        public static ValueConverter Instance { get { return _instance.Value; } }


        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var list = value as IEnumerable;
            if (list != null)
            {
                StringBuilder rt = null;
                foreach (var i in list)
                {
                    if (rt != null)
                        rt.Append(";");
                    else
                        rt = new StringBuilder();
                    rt.Append(i.ToString());                    
                }

                if (rt != null)
                    return rt.ToString();
            }

            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                var str = (string)value;

                string[] array = str.Split(';');

                return new List<string>(array);
            }

            return new List<string>();
        }
    }
}
