using System;
using System.Globalization;
using System.Windows.Data;

namespace CCT.Resource.Converters
{
    public class StringToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;

            if(value.ToString().Equals(string.Empty))
            {
                return false;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
