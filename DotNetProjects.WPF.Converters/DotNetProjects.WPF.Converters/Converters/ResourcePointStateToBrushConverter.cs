using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class EnumerableContainsValueToBrushConverter : IValueConverter
    {
        public Color FalseValue { get; set; }
        public Color TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumerable = value as IEnumerable<string>;
            if (enumerable != null)
            {
                if (enumerable.Any())
                {
                    return new SolidColorBrush(TrueValue);
                }
                return new SolidColorBrush(FalseValue);
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
