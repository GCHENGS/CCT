using CCT.Config;
using CCT.Model.InterFace;
using CCT.Resource.Constants;
using CCT.Resource.Helpers;
using CCT.View;
using CCT.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace CCT.Resource.Dictionarys.ControlStyleResource
{
    public partial class WindowStyle : ResourceDictionary
    {
        #region 窗体操作

        /// <summary>
        /// 窗体移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Drag(sender,e);          
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close(sender);
        }

        /// <summary>
        /// 窗体最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMax_Click(object sender, RoutedEventArgs e)
        {
            Max(sender);
        }

        /// <summary>
        /// 窗体最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            Min(sender);
        }

        #endregion

        #region 中英切换
        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSwitchLanguage_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            // 当前
            var str = btn.Content;
            // 英文
            if (ConstantsForString.En.ToString().Equals(str))
            {
                App.Language = ConstantsForString.Zh_CN.ToString();
            }
            // 中文
            else if (ConstantsForString.Zh.ToString().Equals(str))
            {
                App.Language = ConstantsForString.En_US.ToString();
            }
            App.UpdateLanguage();
        }
        #endregion

        #region 切换皮肤

        /// <summary>
        /// 切换皮换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSwitchSkin_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var btn = sender as Button;
            btn.ContextMenu.Visibility = Visibility.Visible;
            if (btn.ContextMenu.Items.Count == 0)
            {
                foreach (var item in GetSkinCustomMenus())
                {
                    MenuItem menuitem = new MenuItem()
                    {
                        Header = item.Header,
                        IsChecked = item.IsChecked,
                        InputGestureText=item.InputGestureText
                    };
                    menuitem.Click += item.Click;
                    btn.ContextMenu.Items.Add(menuitem);
                }
            }
            //目标
            btn.ContextMenu.PlacementTarget = btn;
            //位置
            btn.ContextMenu.Placement = PlacementMode.Bottom;
            //显示菜单
            btn.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// 单击皮肤MenuItem事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkinMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuitem = sender as MenuItem;
            menuitem.IsChecked = true;
            SwitchSkin(menuitem);
            RecoverMenuItemState(menuitem);
        }

        /// <summary>
        /// 获取皮肤菜单列表
        /// </summary>
        /// <returns></returns>
        private List<ICustomMenu> GetSkinCustomMenus()
        {
            List<ICustomMenu> list = new List<ICustomMenu>();
            list.Add(CustomMenuHelper.CreateCustomMenu("Star Sky", SkinMenuItem_Click, null, null, null, string.Empty, true));
            list.Add(CustomMenuHelper.CreateCustomMenu("Brown Board", SkinMenuItem_Click));
            list.Add(CustomMenuHelper.CreateCustomMenu("Blue Culture", SkinMenuItem_Click));
            list.Add(CustomMenuHelper.CreateCustomMenu("Blue Technology", SkinMenuItem_Click));
            list.Add(CustomMenuHelper.CreateCustomMenu("Green Culture", SkinMenuItem_Click));
            return list;
        }

        /// <summary>
        /// 切换皮肤
        /// </summary>
        /// <param name="menuitem"></param>
        private void SwitchSkin(MenuItem menuitem)
        {
            var parent1 = VisualTreeHelper.GetParent(menuitem);
            while (!(parent1 is ContextMenu))
            {
                parent1 = VisualTreeHelper.GetParent(parent1);
            }
            ContextMenu menu = parent1 as ContextMenu;
            if (menu != null)
            {
                var btn = menu.PlacementTarget as Button;

                var parent2 = VisualTreeHelper.GetParent(btn);

                while (!(parent2 is Grid))
                {
                    parent2 = VisualTreeHelper.GetParent(parent2);
                }
                Grid grid = parent2 as Grid;
                if (grid != null)
                {
                    grid.Tag = menuitem.Header;
                }
            }
        }

        /// <summary>
        /// 点击某个皮肤后恢复其它选中状态
        /// </summary>
        /// <param name="menuitem"></param>
        private void RecoverMenuItemState(MenuItem menuitem)
        {
            var parent = VisualTreeHelper.GetParent(menuitem);
            while (!(parent is ContextMenu))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            ContextMenu menu = parent as ContextMenu;
            if (menu != null)
            {
                foreach (var item in menu.Items)
                {
                    var meitem = item as MenuItem;
                    if (!meitem.Header.Equals(menuitem.Header) && meitem.IsChecked)
                    {
                        meitem.IsChecked = false;
                    }
                }
            }
        }

        #endregion

        #region 界面设置

        /// <summary>
        /// 界面设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSetting_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var btn = sender as Button;
            btn.ContextMenu.Visibility = Visibility.Visible;
            if (btn.ContextMenu.Items.Count == 0)
            {
                foreach (var item in GetSettingCustomMenus())
                {
                    MenuItem menuitem = new MenuItem()
                    {
                        Header = item.Header
                    };
                    menuitem.Click += item.Click;
                    btn.ContextMenu.Items.Add(menuitem);
                }
            }
            //目标
            btn.ContextMenu.PlacementTarget = btn;
            //位置
            btn.ContextMenu.Placement = PlacementMode.Bottom;
            //显示菜单
            btn.ContextMenu.IsOpen = true;
        }

        /// <summary>
        /// 单击设置MenuItem事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContactMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Contact contact = new Contact()
            {
                DataContext = new ContactViewModel()
            };
            contact.ShowDialog();
        }

        /// <summary>
        /// 单击设置MenuItem事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeedbackMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult box = MessageBox.Show("要用浏览器打开URL http://localhost:8080/CCT/feedback 吗？",ConstantsForMessageBox.InfoTip.ToString(), MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (box == MessageBoxResult.Yes)
            {
                Process.Start("http://localhost:8080/CCT/feedback");
            }
        }

        /// <summary>
        /// 单击设置MenuItem事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Help help = new Help()
            {
                DataContext = new HelpViewModel()
            };
            help.ShowDialog();
        }

        /// <summary>
        /// 单击设置MenuItem事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            About about = new About()
            {
                DataContext = new AboutViewModel()
            };
            about.ShowDialog();
        }

        /// <summary>
        /// 获取设置菜单列表
        /// </summary>
        /// <returns></returns>
        private List<ICustomMenu> GetSettingCustomMenus()
        {
            List<ICustomMenu> list = new List<ICustomMenu>();
            list.Add(CustomMenuHelper.CreateCustomMenu("联系我们(ContactUs)", ContactMenuItem_Click));
            list.Add(CustomMenuHelper.CreateCustomMenu("用户反馈(UserFeedBack)", FeedbackMenuItem_Click));
            list.Add(CustomMenuHelper.CreateCustomMenu("帮助(Help)", HelpMenuItem_Click));
            list.Add(CustomMenuHelper.CreateCustomMenu("关于(About)", AboutMenuItem_Click));
            return list;
        }

        #endregion

        #region 执行操作

        /// <summary>
        /// 关闭窗体执行
        /// </summary>
        /// <param name="sender"></param>
        private void Close(object sender)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                Window win = GetWindow(btn);
                if (win != null)
                {
                    //判断是否为主窗体
                    if (win.DataContext is MainWindowViewModel)
                    {
                        var data = win.DataContext as MainWindowViewModel;//获取数据源
                        var richTextBox = VisualTreeHelpers.GetChildObject<RichTextBox>(win, "richTextBox");
                        try
                        {
                            var file = data.CurrentFile;
                            if (file == null)//用户只是登录未处理文件
                            {
                                ConfigHelper.SaveSysConfig(data.SysConfig);//保存系统配置
                                data.UpdateQuiteDate();
                                win.Close();//关闭窗体
                                Environment.Exit(0);//环境退出
                            }
                            else
                            {
                                if (file.IsSave)//已经保存的可以直接关闭
                                {
                                    ConfigHelper.SaveSysConfig(data.SysConfig);//保存系统配置
                                    data.UpdateQuiteDate();
                                    win.Close();//关闭窗体
                                    Environment.Exit(0);//环境退出
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
                                            win.Close();//关闭窗体
                                            Environment.Exit(0);//环境退出
                                        }
                                        else
                                        {
                                            ConfigHelper.SaveSysConfig(data.SysConfig);//保存系统配置
                                            data.UpdateQuiteDate();
                                            win.Close();//关闭窗体
                                            Environment.Exit(0);//环境退出
                                        }
                                    }
                                    else
                                    {
                                        data.SaveCommandExecute(richTextBox);//保存数据
                                        ConfigHelper.SaveSysConfig(data.SysConfig);//保存系统配置
                                        data.UpdateQuiteDate();
                                        win.Close();//关闭窗体
                                        Environment.Exit(0);//环境退出
                                    }
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show("系统关闭异常!");
                        }
                    }else
                    {
                        if (win.DataContext is LoginWindowViewModel)
                        {
                            var data = win.DataContext as LoginWindowViewModel;//获取数据源
                            if(data!=null)
                            {
                                //data.SaveLogin();
                            }
                            win.Close();//关闭窗体
                            Environment.Exit(0);//环境退出
                        }
                        else
                        {
                            win.Close();//关闭弹窗
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 最大化窗体执行
        /// </summary>
        /// <param name="sender"></param>
        private void Max(object sender)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                Window win = GetWindow(btn);
                if (win != null)
                {
                    if (win.WindowState != System.Windows.WindowState.Maximized)
                    {
                        win.WindowState = System.Windows.WindowState.Maximized;
                    }
                    else
                    {
                        win.WindowState = System.Windows.WindowState.Normal;
                    }
                }
            }
        }

        /// <summary>
        /// 最小化窗体执行
        /// </summary>
        /// <param name="sender"></param>
        private void Min(object sender)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                Window win = GetWindow(btn);
                if (win != null)
                {
                    win.WindowState = System.Windows.WindowState.Minimized;
                }
            }
        }

        /// <summary>
        /// 拖动窗体执行
        /// </summary>
        /// <param name="sender"></param>
        private void Drag(object sender, MouseButtonEventArgs e)
        {
            Grid gd = sender as Grid;
            if (gd != null)
            {
                Window win = GetWindow(gd);
                if ( win != null)
                {
                    if (e.ChangedButton == MouseButton.Left)
                        win.DragMove();
                }
            }
        }

        /// <summary>
        /// 获取窗体对象
        /// </summary>
        /// <returns></returns>
        private Window GetWindow(DependencyObject obj)
        {
            var parent = VisualTreeHelper.GetParent(obj);
            while (!(parent is Window))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return  parent as Window;
        }

        #endregion
    }
}
