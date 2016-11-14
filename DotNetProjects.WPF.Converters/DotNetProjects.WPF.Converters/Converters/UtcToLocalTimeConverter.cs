using System;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class UtcToLocalTimeConverter: ValueConverter
    {
        private static readonly Lazy<ValueConverter> _instance = new Lazy<ValueConverter>(() => new UtcToLocalTimeConverter());
        public static ValueConverter Instance { get { return _instance.Value; } }


        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value == DBNull.Value)
                return null;

            var dt = ((DateTime) value);

            if (dt.Kind == DateTimeKind.Utc)
                return dt.ToLocalTime();
            return dt;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            DateTime dt;
            var s = value as string;
            if (s != null)
            {
                dt = DateTime.Parse(s);
            }
            else
            {
                dt = (DateTime)value;
            }

            if (dt.Kind != DateTimeKind.Utc)
                return dt.ToUniversalTime();
            return dt;
        }
    }
}
