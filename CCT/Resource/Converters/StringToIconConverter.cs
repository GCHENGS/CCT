using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CCT.Resource.Converters
{
    public class StringToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           Image icon = new Image();
           if (value == null)
           {
              icon.Source = new BitmapImage(new Uri(@"/Resource/Images/headIcon.png", UriKind.Relative));
            }
            else
            {
                var path = "D:/JavaEE/CCT/WebContent" + value.ToString().Substring(1,value.ToString().Length-1);
                icon.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            }
            return icon.Source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
