using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class EnumFlagsValue<T>
    {
        public string EnumString { get; set; }
        public T Value { get; set; }
    }

    [ContentProperty("EnumValues")]
    public class EnumFlagsConverter<T> : ValueConverter
    {
        public Type EnumType { get; set; }

        private string _enumTypeString;
        public string EnumTypeString
        {
            get { return _enumTypeString; }
            set
            {
                _enumTypeString = value;

                EnumType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).FirstOrDefault(x => x.Name == value);
#if SILVERLIGHT
                if (EnumType == null)
                    EnumType = CommonClient.LoadedAssemblys.SelectMany(s => s.GetTypes()).FirstOrDefault(x => x.Name == value);
#endif
            }
        }

        public T DefaultValue { get; set; }

        private ObservableCollection<EnumFlagsValue<T>> _enumValues = new ObservableCollection<EnumFlagsValue<T>>();
        public IList EnumValues
        {
            get { return this._enumValues; }            
        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return DefaultValue;

            if (EnumType != null && value.GetType() != EnumType)
            {
                if (value is string)
                    value = Enum.Parse(EnumType, value.ToString(), true);
                else
                    value = Enum.ToObject(EnumType, value);
            }

            foreach (EnumFlagsValue<T> enumValue in this.EnumValues)
            {
                var wr = Enum.Parse(EnumType, enumValue.EnumString, true) as Enum;

                if (((Enum) value).HasFlag(wr))
                    return enumValue.Value;
            }

            return DefaultValue;          
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EnumFlagsToVisibilityConverter : EnumFlagsConverter<Visibility> { } ;
    public class EnumFlagsValueVisibility : EnumFlagsValue<Visibility> { } ;
}
