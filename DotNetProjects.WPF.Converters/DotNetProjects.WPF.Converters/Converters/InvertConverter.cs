using System;

namespace DotNetProjects.WPF.Converters.Converters
{
    class InvertConverter : ValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return -(double)value;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
