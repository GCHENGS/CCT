using CCT.Model.DataType;
using CCT.Model.InterFace;
using System.Windows;
using System.Windows.Input;

namespace CCT.Resource.Helpers
{
    public class CustomMenuHelper
    {
        #region 创建菜单项

        public static ICustomMenu CreateCustomMenu(string header, RoutedEventHandler click = null, ICommand command = null, object commandParameter = null, object icon = null, string inputGestureText = "", bool isChecked = false)
        {
            return new CustomMenu()
            {
                Icon = icon,
                Header = header,
                InputGestureText = inputGestureText,
                IsChecked = isChecked,
                Click = click,
                Command = command,
                CommandParameter = commandParameter
            };
        }

        #endregion
    }
}
