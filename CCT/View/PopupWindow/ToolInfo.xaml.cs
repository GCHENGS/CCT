using System.Windows;

namespace CCT.View
{
    /// <summary>
    /// ToolInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ToolInfo : Window
    {
        public ToolInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 确定关闭弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
