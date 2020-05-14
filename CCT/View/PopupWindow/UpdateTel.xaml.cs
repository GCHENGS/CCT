using CCT.Resource.Helpers;
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
    /// UpdateTel.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateTel : Window
    {
        public UpdateTel()
        {
            InitializeComponent();        
            newTel.Focus();
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

        /// <summary>
        /// 检测输入是否为数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newTel_KeyUp(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;

            var str = textBox.Text;

            if (!RegexHelper.IsNumber(textBox.Text))
            {
                textBox.Text = str.Substring(0, str.Length - 1);
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        /// <summary>
        /// 检测长度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newTel_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;

            if (textBox.Text.Length==11)
            {
                MessageBox.Show("长度已经达到11位,不能再输入!","信息提示",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }
    }
}
