using System;
using System.Globalization;
using System.Windows.Controls;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class ItemLineNumbersConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as ListBoxItem;
            var view = (ListBox)ItemsControl.ItemsControlFromItemContainer(item);
            return view.ItemContainerGenerator.IndexFromContainer(item) + 1;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
