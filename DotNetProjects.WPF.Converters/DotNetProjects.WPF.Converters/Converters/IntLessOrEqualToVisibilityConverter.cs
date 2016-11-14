using System;
using System.Windows;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class IntLessOrEqualToVisibilityConverter : ValueConverter
    {
        public int Value { get; set; }

        private Visibility _visibilityWhenLessOrEqual = Visibility.Collapsed;
        public Visibility VisibilityWhenLessOrEqual
        {
            get { return _visibilityWhenLessOrEqual; }
            set { _visibilityWhenLessOrEqual = value; }
        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (System.Convert.ToInt32(value) <= Value)
                return VisibilityWhenLessOrEqual;


            if (VisibilityWhenLessOrEqual == Visibility.Visible)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
