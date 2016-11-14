using System;

namespace DotNetProjects.WPF.Converters.Converters
{
    public class StringToConverter : ValueConverter
    {
        public string CompareValue { get; set; }

        public enum CompareType
        {
            Equals,
            EqualsNot,
            EndsWith,
            EndsNotWith,
            StartsWith,
            StartsNotWith
        }

        public CompareType CompareEnum { get; set; }        
    }

    public class StringToBoolConverter : StringToConverter
    {
        private static readonly Lazy<ValueConverter> _instance = new Lazy<ValueConverter>(() => new StringToBoolConverter());
        public static ValueConverter Instance { get { return _instance.Value; } }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (CompareValue == null)
                    CompareValue = "";

                if (CompareEnum == CompareType.Equals)
                {
                    return value.Equals(CompareValue);
                }
                if (CompareEnum == CompareType.EndsWith)
                {
                    return value.ToString().EndsWith(CompareValue);
                }
                if (CompareEnum == CompareType.StartsWith)
                {
                    return value.ToString().StartsWith(CompareValue);
                }

                if (CompareEnum == CompareType.EqualsNot)
                {
                    return !value.Equals(CompareValue);
                }
                if (CompareEnum == CompareType.EndsNotWith)
                {
                    return !value.ToString().EndsWith(CompareValue);
                }
                if (CompareEnum == CompareType.StartsNotWith)
                {
                    return !value.ToString().StartsWith(CompareValue);
                }
            }

            return false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }   
}