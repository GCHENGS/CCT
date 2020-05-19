using CCT.Resource.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CCT.Resource.Converters
{
    /// <summary>
    /// 实现UI控件绑定多属性信息并返回自定义格式
    /// </summary>
    public class MultiBindingDataConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return DependencyProperty.UnsetValue;
            if (parameter == null) return values.ToString();
            MultiBindingFormatType m;
            try
            {
                m = (MultiBindingFormatType)Enum.Parse(typeof(MultiBindingFormatType),parameter.ToString());
                switch(m)
                {
                    case MultiBindingFormatType.SectionFormat: return "Section[" + values[0] + "]";
                    case MultiBindingFormatType.CommonFormat:return values[0] +"(" +values[1] + ")";
                    case MultiBindingFormatType.CommonWithSubFormat:return values[0] + "(" + values[1] + "):" + values[2] + "(" + values[3] + ")";
                    default:return values.ToString();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { value, Binding.DoNothing };
        }
    }
}
