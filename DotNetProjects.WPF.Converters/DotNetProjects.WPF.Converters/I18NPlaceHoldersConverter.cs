using System;
using System.Globalization;
using System.Windows.Data;

namespace DotNetProjects.WPF.Converters
{

    public class I18NPlaceHoldersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format(value.ToString(), parameter as string[]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
