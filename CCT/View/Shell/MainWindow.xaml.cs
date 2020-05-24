using CCT.Config;
using CCT.Resource.Constants;
using CCT.Resource.Helpers;
using CCT.Resource.Helpers.InterFace;
using CCT.ViewModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CCT.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 构造方法

        public MainWindow()
        {
            InitializeComponent();

            AtColor = BaseColor;
            BgColor = BaseColor;
        }

        #endregion

        #region 编辑

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void undo_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox?.CanUndo == true)
            {
                if (BgColor == BaseColor)
                {
                    richTextBox.Undo();
                    return;
                }
                richTextBox.Background = BgColor;
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void redo_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox?.CanRedo == true)
            {
                if (BgColor == BaseColor)
                {
                    richTextBox.Redo();
                    return;
                }
                richTextBox.Background = BgColor;
            }
        }

        /// <summary>
        /// 剪切
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cut_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null) return;

            if (!richTextBox.Selection.IsEmpty)
            {
                richTextBox.Cut();
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copy_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null) return;

            if (!richTextBox.Selection.IsEmpty)
            {
                richTextBox.Copy();
            }
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paste_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null) return;

            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                richTextBox.Paste();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null) return;

            if (!richTextBox.Selection.IsEmpty)
            {
                richTextBox.Selection.Text = string.Empty;
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectAll_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null) return;

            richTextBox.SelectAll();
        }

        #endregion

        #region 字体

        /// <summary>
        /// 字体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ant_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null) return;
            System.Windows.Forms.FontDialog fontDialog = new System.Windows.Forms.FontDialog();
            fontDialog.ShowColor = true;
            fontDialog.ShowApply = false;
            fontDialog.ShowEffects = true;
            if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Drawing.FontFamily DFamily = fontDialog.Font.FontFamily;
                System.Windows.Media.FontFamily MFamily = new System.Windows.Media.FontFamily(DFamily.Name);
                richTextBox.FontFamily = MFamily;
                System.Drawing.Color DColor = fontDialog.Color;
                System.Windows.Media.Color MColor = new System.Windows.Media.Color();
                MColor = System.Windows.Media.Color.FromArgb(DColor.A, DColor.R, DColor.G, DColor.B);
                System.Windows.Media.Brush BColor = new System.Windows.Media.SolidColorBrush(MColor);
                richTextBox.Foreground = BColor;
            }
        }

        /// <summary>
        /// 加粗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null) return;

            if (richTextBox.FontWeight == FontWeights.Bold)
            {
                richTextBox.FontWeight = FontWeights.Regular;
                Bold.IsChecked = false;
                //Bold.FontWeight = FontWeights.Regular;
            }
            else
            {
                richTextBox.FontWeight = FontWeights.Bold;
                Bold.IsChecked = true;
                //Bold.FontWeight = FontWeights.Bold;
            }
        }

        /// <summary>
        /// 斜体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null) return;

            if (richTextBox.FontStyle == FontStyles.Italic)
            {
                richTextBox.FontStyle = FontStyles.Normal;
                Italic.IsChecked = false;
                //Italic.FontWeight = FontWeights.Regular;
            }
            else
            {
                richTextBox.FontStyle = FontStyles.Italic;
                Italic.IsChecked = true;
                //Italic.FontWeight = FontWeights.Bold;
            }
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AntColor_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            colorDialog.AnyColor = true;
            colorDialog.AllowFullOpen = true;
            colorDialog.FullOpen = true;
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Drawing.Color DColor = colorDialog.Color;
                System.Windows.Media.Color MColor = new System.Windows.Media.Color();
                MColor = System.Windows.Media.Color.FromArgb(DColor.A, DColor.R, DColor.G, DColor.B);
                AtColor = new System.Windows.Media.SolidColorBrush(MColor);
                richTextBox.Foreground = AtColor;
            }
        }

        #endregion

        #region 工具

        /// <summary>
        /// 窗口置顶
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void top_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Topmost)
            {
                this.Topmost = true;
            }
        }

        /// <summary>
        /// 自动换行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoEnter_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null) return;
            if (autoEnter.IsChecked)
            {
                autoEnter.IsChecked = false;
                richTextBox.AcceptsReturn = false;
            }
            else
            {
                autoEnter.IsChecked = true;
                richTextBox.AcceptsReturn = true;
            }
        }

        /// <summary>
        /// 查看状态栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void watchStatus_Click(object sender, RoutedEventArgs e)
        {
            if (watchStatus.IsChecked)
            {
                watchStatus.IsChecked = false;
                status.Visibility = Visibility.Collapsed;
            }
            else
            {
                watchStatus.IsChecked = true;
                status.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 背景颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgColor_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null) return;
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            colorDialog.AnyColor = true;
            colorDialog.AllowFullOpen = true;
            colorDialog.FullOpen = true;
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Drawing.Color DColor = colorDialog.Color;
                System.Windows.Media.Color MColor = new System.Windows.Media.Color();
                MColor = System.Windows.Media.Color.FromArgb(DColor.A, DColor.R, DColor.G, DColor.B);
                BgColor = new System.Windows.Media.SolidColorBrush(MColor);
                richTextBox.Background = BgColor;
            }
        }

        /// <summary>
        /// 查找替换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void replace_Click(object sender, RoutedEventArgs e)
        {
            Replace win = new Replace();
            win.Owner = this;
            win.Show();
        }

        /// <summary>
        /// 查找下一个
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectNext_Click(object sender, RoutedEventArgs e)
        {
            SelectNext win = new SelectNext();
            win.Owner = this;
            win.Show();
        }

        /// <summary>
        /// 选中项大写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gotoA_Click(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null) return;

            if (!richTextBox.Selection.IsEmpty)
            {
                //调用处理文字方法，并重新设置给 文本框
                Iplug iplug = new PlugToUpper();
                richTextBox.Selection.Text = iplug.ProcessText(this.richTextBox.Selection.Text);
            }
        }

        /// <summary>
        /// 选中项小写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gotoa_Click_1(object sender, RoutedEventArgs e)
        {
            if (richTextBox == null) return;

            if (!richTextBox.Selection.IsEmpty)
            {
                //调用处理文字方法，并重新设置给 文本框
                Iplug iplug = new PlugToLower();
                richTextBox.Selection.Text = iplug.ProcessText(this.richTextBox.Selection.Text);
            }
        }

        #endregion

        #region 帮助

        /// <summary>
        /// 工具简介
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolInfo_Click(object sender, RoutedEventArgs e)
        {
            ToolInfo info = new ToolInfo()
            {
                DataContext = new ToolInfoViewModel()
            };
            info.Owner = this;
            info.ShowDialog();
        }

        /// <summary>
        /// CCT主页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void index_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult box = MessageBox.Show("要用浏览器打开URL http://localhost:8080/CCT/index.jsp 吗？", ConstantsForMessageBox.InfoTip.ToString(), MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (box == MessageBoxResult.Yes)
            {
                Process.Start("http://localhost:8080/CCT/index.jsp");
            }
        }

        /// <summary>
        /// 用户反馈
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void feedback_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult box = MessageBox.Show("要用浏览器打开URL http://localhost:8080/CCT/feedback 吗？", ConstantsForMessageBox.InfoTip.ToString(), MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (box == MessageBoxResult.Yes)
            {
                Process.Start("http://localhost:8080/CCT/feedback");
            }
        }
        /// <summary>
        /// 联系我们
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contact_Click(object sender, RoutedEventArgs e)
        {
            Contact contact = new Contact()
            {
                DataContext = new ContactViewModel()
            };
            contact.Owner = this;
            contact.ShowDialog();
        }

        /// <summary>
        /// 关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void about_Click(object sender, RoutedEventArgs e)
        {
            About about = new About()
            {
                DataContext = new AboutViewModel()
            };
            about.Owner = this;
            about.ShowDialog();
        }

        /// <summary>
        /// 帮助
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void help_Click(object sender, RoutedEventArgs e)
        {
            Help help = new Help()
            {
                DataContext = new HelpViewModel()
            };
            help.Owner = this;
            help.ShowDialog();
        }

        #endregion

        #region 帐户

        /// <summary>
        /// 切换帐户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void switchAccount_Click(object sender, RoutedEventArgs e)
        {
            var data = DataContext as MainWindowViewModel;
            try
            {
                var file = data.CurrentFile;
                if (file == null)//用户只是登录未处理文件
                {
                    ConfigHelper.SaveSysConfig(data.SysConfig);//保存系统配置
                    data.UpdateQuiteDate();
                    this.Close();//关闭窗体
                }
                else
                {
                    if (file.IsSave)//已经保存的可以直接关闭
                    {
                        ConfigHelper.SaveSysConfig(data.SysConfig);//保存系统配置
                        data.UpdateQuiteDate();
                        this.Close();//关闭窗体
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(file.FilePath))
                        {
                            MessageBoxResult result = MessageBox.Show("存在未保存的内容，是否需要保存？", "信息提示", MessageBoxButton.YesNo, MessageBoxImage.Information);
                            if (result == MessageBoxResult.Yes)
                            {
                                data.SaveCommandExecute(richTextBox);//保存数据
                                ConfigHelper.SaveSysConfig(data.SysConfig);//保存系统配置
                                data.UpdateQuiteDate();
                                this.Close();//关闭窗体
                            }
                            else
                            {
                                ConfigHelper.SaveSysConfig(data.SysConfig);//保存系统配置
                                data.UpdateQuiteDate();
                                this.Close();//关闭窗体
                            }
                        }
                        else
                        {
                            data.SaveCommandExecute(richTextBox);//默认保存数据
                            ConfigHelper.SaveSysConfig(data.SysConfig);//保存系统配置
                            data.UpdateQuiteDate();
                            this.Close();//关闭窗体
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("系统关闭异常!");
                return;
            }
            LoginWindow login = new LoginWindow();
            {
                DataContext = new LoginWindowViewModel();
            }
            login.Show();
        }

        #endregion

        #region 私有变量

        private System.Windows.Media.Brush BaseColor = new System.Windows.Media.SolidColorBrush();

        private System.Windows.Media.Brush AtColor;//字体颜色

        private System.Windows.Media.Brush BgColor;//背景颜色

        #endregion

        #region 最近文件

        /// <summary>
        /// 最近文件列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var path = menuItem.Tag.ToString();
            var data = (DataContext as MainWindowViewModel);
            if(data!=null)
            {
                data.OpenFilePath(path,richTextBox);
            }
        }

        #endregion
    }
}
