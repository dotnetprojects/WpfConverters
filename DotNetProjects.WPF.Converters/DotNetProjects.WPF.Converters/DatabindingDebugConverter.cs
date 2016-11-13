using System;
using System.Diagnostics;
using System.Globalization;

namespace DotNetProjects.WPF.Converters
{
    /// <summary>
    /// This converter does nothing except breaking the
    /// debugger into the convert method
    /// </summary>
    public class DatabindingDebugConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            Debugger.Break();
            return value;
        }

        public override object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            Debugger.Break();
            return value;
        }
    }
}
