using CCT.Config;
using CCT.Model.DataType;
using CCT.Resource.Constants;
using CCT.Service;
using CCT.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CCT.View
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        #region 局部变量

        /// <summary>
        /// 定义系统配置
        /// </summary>
        private SysConfig SysConfig;

        /// <summary>
        /// 定义用户
        /// </summary>
        private User User;

        /// <summary>
        /// 定义上次登录
        /// </summary>
        private SavedLastLoginUser SavedLastLoginUser;

        /// <summary>
        /// 定义上次操作
        /// </summary>
        private SaveUserOperator SaveUserOperator;

        /// <summary>
        /// 后台执行
        /// </summary>
        System.ComponentModel.BackgroundWorker bw = new System.ComponentModel.BackgroundWorker
        {
            WorkerSupportsCancellation = true,//支持取消后台操作
        };

        /// <summary>
        /// 登陆信息的反馈值
        /// </summary>
        int feedBack = 0;

        /// <summary>
        /// 取消后台操作
        /// </summary>
        bool cancellationOperation = false;

        #endregion

        #region 构造方法

        public LoginWindow()
        {
            InitializeComponent();
            this.Loaded += LoginWindow_Loaded;
            //bw.DoWork += bw_DoWork;
            //bw.RunWorkerCompleted += bw_RunWorkerCompleted;
        }

        #endregion

        #region 注册链接

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegistLink_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult box = MessageBox.Show("要用浏览器打开URL http://localhost:8080/CCT/regist 吗？", ConstantsForMessageBox.InfoTip.ToString(), MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (box == MessageBoxResult.Yes)
            {
                Process.Start("http://localhost:8080/CCT/regist");
            }
        }

        #endregion

        #region 忘记密码

        /// <summary>
        /// 找回事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FoundLink_Click(object sender, RoutedEventArgs e)
        {
            Contact contact = new Contact()
            {
                DataContext = new ContactViewModel()
            };
            contact.Owner = this;
            contact.ShowDialog();
        }

        #endregion

        #region 登录处理

        /// <summary>
        /// 登陆后台运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            feedBack = Verification();
        }

        /// <summary>
        /// 登陆操作后台运行结束后操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bw_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //取消后台登录
            if (this.cancellationOperation)
            {
                this.cancellationOperation = false;//恢复以前状态
                return;//截断
            }

            switch (feedBack)
            {
                case 1:
                    //登录成功
                    SaveLogin();
                    UpdateLoginDate();
                    //生成主窗体
                    MainWindow mainWindow = new MainWindow()
                    {
                        DataContext = new MainWindowViewModel(User)
                    };
                    //设置系统主窗体
                    App.Current.MainWindow = mainWindow;
                    //关闭登陆界面
                    this.Close();
                    //显示主窗体
                    mainWindow.Show();
                    break;
                case 0:
                    MessageBox.Show("登录失败！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case -1:
                    System.Windows.MessageBox.Show("数据库未连接！", "系统提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    break;
                case -2:
                    System.Windows.MessageBox.Show("用户名不能为空！", "系统提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    break;
                case -3:
                    System.Windows.MessageBox.Show("密码不能为空！", "系统提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    break;         
                case -4:
                    System.Windows.MessageBox.Show("用户名或密码不正确！", "系统提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    break; 
                default:
                    MessageBox.Show("未知错误！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
            //this.LoadGrid.Visibility = System.Windows.Visibility.Collapsed;//遮罩层隐藏
        }

        #endregion

        #region 登录验证处理

        /// <summary>
        /// 用于验证登陆信息
        /// </summary>
        /// <returns>
        /// -4：用户名或密码不正确 
        /// -3：密码不能为空
        /// -2：用户名不能为空
        /// -1：数据库未连接 
        ///  0：登陆失败
        ///  1：登陆成功
        /// </returns>
        public int Verification()
        {
            int flag = 0;
            try
            {
                if (string.IsNullOrEmpty(User.UserName))
                {
                    flag = -2;
                    return flag;
                }
                if (string.IsNullOrEmpty(User.UserPassword))
                {
                    flag = -3;
                    return flag;
                }
                User user = UserService.Login(User);//服务方法
                //验证登录
                if (user == null)
                {
                    flag = -4;
                }
                else
                {
                    flag = 1;
                    User = user;
                    //休眠1.5秒
                    System.Threading.Thread.Sleep(1500);
                }
            }
            catch (Exception)
            {
                flag = -1;
            }
            return flag;
        }

        #endregion

        #region 保存登录信息

        /// <summary>
        /// 保存登录信息到本地
        /// </summary>
        private void SaveLogin()
        {
            SavedLastLoginUser.X1 = User.UserName;
            SavedLastLoginUser.X2 = User.UserPassword;
            SaveUserOperator.X1 = ReAccount.IsChecked.ToString();
            SaveUserOperator.X2 = AutoLogin.IsChecked.ToString();
            SysConfig.SavedLastLoginUser = SavedLastLoginUser;
            ConfigHelper.SaveSysConfig(SysConfig);
        }

        #endregion

        #region 更新登录时间

        /// <summary>
        /// 更新登录信息到数据库
        /// </summary>
        private void UpdateLoginDate()
        {
            User.UserLoginDate = DateTime.Now;
            UserService.UpdateUserLoginDate(User);
        }

        #endregion

        #region 窗体加载

        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //界面中浮云移动动画
            Storyboard sbd = Resources["sbCloud"] as Storyboard;
            sbd.Begin();

            //获取登录信息
            SysConfig = ConfigHelper.ReadSysConfig();
            SavedLastLoginUser = SysConfig.SavedLastLoginUser;
            SaveUserOperator = SysConfig.SaveUserOperator;
            User = new User()
            {
                UserName = SavedLastLoginUser.X1,
                UserPassword = SavedLastLoginUser.X2,  
            };
            if (SaveUserOperator.X1.ToLower() == "false")
            {
                User.RememberPwd = false;
            }
            else
            {
                User.RememberPwd = true;
            }
            if (SaveUserOperator.X2.ToLower() == "false")
            {
                User.AutomaticLogon = false;
            }
            else
            {
                User.AutomaticLogon = true;
            }

            //计时器 自动登录做准备的数据计时执行
            var timer2 = new System.Timers.Timer
            {
                Interval = 400,
            };
            timer2.Elapsed += new System.Timers.ElapsedEventHandler(theout2);
            timer2.AutoReset = false;
            timer2.Enabled = true;
        }

        #endregion

        #region 自动登录

        /// <summary>
        /// 自动登录
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void theout2(object source, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                //显示上次登录
                if (SavedLastLoginUser != null)
                {
                    if (SaveUserOperator.X2.ToLower() == "true")//自动登录
                    {
                        //触发登录
                        //this.LoginButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, this.LoginButton));
                    }
                }
            }));
        }

        /// <summary>
        /// 登录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.LoadGrid.Visibility = System.Windows.Visibility.Visible;//遮罩层可见
                //bw.RunWorkerAsync();
            }
            catch (Exception)
            {
                //报错无视
            }
        }

        #endregion

        /*#region 登陆加载界面局部变量
        /// <summary>
        /// 队列计时器
        /// </summary>
        private DispatcherTimer animationTimer;
        #endregion

        #region 登陆加载界面初始化方法

        public void LoadingWait()
        {
            animationTimer = new DispatcherTimer(
                DispatcherPriority.ContextIdle, Dispatcher);
            animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 90);
        }
        #endregion

        #region 执行方法

        private void Start()
        {
            animationTimer.Tick += HandleAnimationTick;
            animationTimer.Start();
        }

        private void Stop()
        {
            animationTimer.Stop();
            animationTimer.Tick -= HandleAnimationTick;
        }

        private void HandleAnimationTick(object sender, EventArgs e)
        {
            SpinnerRotate.Angle = (SpinnerRotate.Angle + 36) % 360;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            const double offset = Math.PI;
            const double step = Math.PI * 2 / 10.0;

            SetPosition(C0, offset, 0.0, step);
            SetPosition(C1, offset, 1.0, step);
            SetPosition(C2, offset, 2.0, step);
            SetPosition(C3, offset, 3.0, step);
            SetPosition(C4, offset, 4.0, step);
            SetPosition(C5, offset, 5.0, step);
            SetPosition(C6, offset, 6.0, step);
            SetPosition(C7, offset, 7.0, step);
            SetPosition(C8, offset, 8.0, step);
        }

        private void SetPosition(Ellipse ellipse, double offset,
            double posOffSet, double step)
        {
            ellipse.SetValue(Canvas.LeftProperty, 50.0
                + Math.Sin(offset + posOffSet * step) * 50.0);

            ellipse.SetValue(Canvas.TopProperty, 50
                + Math.Cos(offset + posOffSet * step) * 50.0);
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void HandleVisibleChanged(object sender,
            DependencyPropertyChangedEventArgs e)
        {
            bool isVisible = (bool)e.NewValue;
            if (animationTimer == null)
            {
                LoadingWait();
            }
            if (isVisible)
                Start();
            else
                Stop();
        }

        /// <summary>
        /// 取消登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Yes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.cancellationOperation = true;
            bw.CancelAsync();//取消后台操作
            this.LoadGrid.Visibility = System.Windows.Visibility.Collapsed;//遮罩层隐藏
        }

        #endregion*/
    }
}
