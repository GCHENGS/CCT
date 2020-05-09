using CCT.Model.InterFace;
using System.Windows;
using System.Windows.Input;

namespace CCT.Model.DataType
{
    public class CustomMenu : ICustomMenu
    {
        public RoutedEventHandler Click { get; set; }

        public ICommand Command { get; set; }

        public object CommandParameter { get; set; }

        public string Header{ get; set; }

        public object Icon { get; set; }

        public string InputGestureText { get; set; }

        public bool IsChecked { get; set; }
    }
}
