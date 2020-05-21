using CCT.Model.DataType;
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
    /// JsonParseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class JsonParseWindow : Window
    {
        public JsonParseWindow()
        {
            InitializeComponent();
        }

        #region 编辑区域

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void undo_Click(object sender, RoutedEventArgs e)
        {
            if (xml?.CanUndo == true)
            {
                xml.Undo();
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void redo_Click(object sender, RoutedEventArgs e)
        {
            if (xml?.CanRedo == true)
            {
                xml.Redo();
            }
        }

        /// <summary>
        /// 剪切
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cut_Click(object sender, RoutedEventArgs e)
        {
            if (xml == null) return;

            if (!xml.Selection.IsEmpty)
            {
                xml.Cut();
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copy_Click(object sender, RoutedEventArgs e)
        {
            if (xml == null) return;

            if (!xml.Selection.IsEmpty)
            {
                xml.Copy();
            }
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paste_Click(object sender, RoutedEventArgs e)
        {
            if (xml == null) return;

            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                xml.Paste();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (xml == null) return;

            if (!xml.Selection.IsEmpty)
            {
                xml.Selection.Text = string.Empty;
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectAll_Click(object sender, RoutedEventArgs e)
        {
            if (xml == null) return;

            xml.SelectAll();
        }

        #endregion

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

        #region 树切换选中

        private void json_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tree = sender as TreeView;
            var node = tree.SelectedItem as Node;
            var data = (DataContext as JsonParseWindowViewModel);
            if (data != null)
            {
                data.CurrentNode = node;
            }
        }

        #endregion
    }
}
