using System;
using System.Globalization;
using System.Windows.Data;

namespace DotNetProjects.WPF.Converters
{
    public class DateTimeToLongTimeStringConverter : IValueConverter
    {
        private const string NoDate = "";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                var date = (DateTime) value;
                if (date == DateTime.MinValue)
                    return NoDate;
                return date.ToLongTimeString();
            }
            return NoDate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
