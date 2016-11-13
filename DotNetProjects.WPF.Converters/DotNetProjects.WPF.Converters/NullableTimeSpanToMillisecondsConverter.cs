using System;
using System.Windows.Data;

namespace DotNetProjects.WPF.Converters
{
    public class NullableTimeSpanToMillisecondsConverter : IValueConverter
    {       
        //NullableTimeSpan--> Milliseconds
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var typedValue = value as TimeSpan?;            
            var factor = System.Convert.ToInt32(parameter);

            if (typedValue == null)
                return null;

            if (factor > 0)
                return typedValue.Value.TotalMilliseconds / factor;
            else
                return typedValue.Value.TotalMilliseconds;
        }

        //Milliseconds --> NullableTimeSpan
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {            
            TimeSpan? timeSpan = null;
            var factor = System.Convert.ToInt32(parameter);

            if (value != null)
            {
                var typedValue = System.Convert.ToDouble(value);

                if (typedValue > 0)
                {
                    if (factor > 0)
                        typedValue = (int) (factor * typedValue);

                    timeSpan = TimeSpan.FromMilliseconds(typedValue);   
                }                    
            }

            return timeSpan;
        }
    }
}
