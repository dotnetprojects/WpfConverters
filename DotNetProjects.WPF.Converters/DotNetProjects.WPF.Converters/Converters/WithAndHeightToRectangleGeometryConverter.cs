using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class WithAndHeightToRectangleGeometryConverter : IMultiValueConverter
    {
        public int RadiusX { get; set; }
        public int RadiusY { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] != DependencyProperty.UnsetValue && values[1] != DependencyProperty.UnsetValue)
            {
                if (double.IsNaN((double)values[0]) || double.IsNaN((double)values[1]))
                    return null;
                return new RectangleGeometry() {RadiusX =this.RadiusX, RadiusY = this.RadiusY, Rect = new Rect(0, 0, System.Convert.ToInt32(values[0]), System.Convert.ToInt32(values[1])) };
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}   
