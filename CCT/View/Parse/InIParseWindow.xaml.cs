using CCT.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CCT.View
{
    /// <summary>
    /// InIParseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InIParseWindow : Window
    {
        public InIParseWindow()
        {
            InitializeComponent();
        }

        #region 文本框状态

        private void TextBox_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox txt = (sender as TextBox);
            txt.IsReadOnly = false;
            txt.BorderThickness = new Thickness(1);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txt = (sender as TextBox);
            txt.IsReadOnly = true;
            txt.BorderThickness = new Thickness();
        }

        #endregion
    }
}
