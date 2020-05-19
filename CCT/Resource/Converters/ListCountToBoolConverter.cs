using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace CCT.Resource.Converters
{
    public class ListCountToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            if (value is IList)
            {
                if ((value as IList).Count == 0)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
