using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CCT.Resource.Converters
{
    public class TagToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bgSource = "/Resource/Images/bg0.png";
            if (value == null) return bgSource;     
            switch(value.ToString())
            {
                case "Star Sky": bgSource = "/Resource/Images/bg0.png";break;
                case "Brown Board": bgSource = "/Resource/Images/bg1.png";break;
                case "Blue Culture": bgSource = "/Resource/Images/bg2.png";break;
                case "Blue Technology": bgSource = "/Resource/Images/bg3.png";break;
                case "Green Culture": bgSource = "/Resource/Images/bg4.png";break;
                default:break;
            }
            return bgSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
