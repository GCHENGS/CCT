using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CCT.Resource.Converters
{
    public class CornerRadiusToThickness : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness result = new Thickness();
            if (value is CornerRadius)
            {
                var tem = (CornerRadius)value;
                result = new Thickness(tem.TopLeft,tem.BottomLeft,tem.TopRight,tem.BottomRight);
            }
            return result;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
