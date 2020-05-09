using System.Windows;
using System.Windows.Input;

namespace CCT.Model.InterFace
{
    public interface ICustomMenu
    {
        object Icon { get; set; }

        string Header { get; set; }

        RoutedEventHandler Click { get; set; }

        ICommand Command { get; set; }

        object CommandParameter { get; set; }

        string InputGestureText { get; set; }

        bool IsChecked { get; set; }
    }
}
