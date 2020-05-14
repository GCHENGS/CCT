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
    /// UpdatePwd.xaml 的交互逻辑
    /// </summary>
    public partial class UpdatePwd : Window
    {
        public UpdatePwd()
        {
            InitializeComponent();
            oldPwd.Focus();
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
