using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters
{
    public class DiagnosisMessageBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var message = value as string;

            if (message != null)           
            {
                if (message.Contains("(Exception)"))
                {
                    return new SolidColorBrush(Colors.Red);
                }
                if (message.Contains("(Error)"))
                {
                    return new SolidColorBrush(Colors.Red);
                }

                if (message.Contains("(Warning)"))
                {
                    return new SolidColorBrush(Colors.Orange);
                }
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
