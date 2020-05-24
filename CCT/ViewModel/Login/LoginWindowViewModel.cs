using CCT.Config;
using CCT.Model.DataType;
using CCT.Service;
using CCT.View;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace CCT.ViewModel
{
    /// <summary>
    /// 登录窗体图模型
    /// </summary>
    public class LoginWindowViewModel : ViewModelBase
    {
        #region 私有域

        private User currentUser;//用户
        private string userName;//名称
        private string userPassword;//密码
        private bool isCurrentUserNull = true ;//是否用户为空
        private bool isReAccount = false;//是否记住密码
        private bool isAutoLogin = false;//是否自动登录

        #endregion

        #region 属性

        /// <summary>
        /// 用户
        /// </summary>
        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                SetProperty(ref currentUser, value);
                if(currentUser!=null)
                {
                    IsCurrentUserNull = false;
                }
                else
                {
                    IsCurrentUserNull = true;
                }
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string UserPassword
        {
            get { return userPassword; }
            set { SetProperty(ref userPassword, value); }
        }

        /// <summary>
        /// 是否用户为空
        /// </summary>
        public bool IsCurrentUserNull
        {
            get { return isCurrentUserNull; }
            set { SetProperty(ref isCurrentUserNull, value); }
        }

        /// <summary>
        /// 是否记住密码
        /// </summary>
        public bool IsReAccount
        {
            get { return isReAccount; }
            set { SetProperty(ref isReAccount, value); }
        }

        /// <summary>
        /// 是否自动登录
        /// </summary>
        public bool IsAutoLogin
        {
            get { return isAutoLogin; }
            set
            {
                SetProperty(ref isAutoLogin, value);
                if(isAutoLogin)
                {
                    if (!isReAccount) IsReAccount = true;
                }
            }
        }

        /// <summary>
        /// 定义系统配置
        /// </summary>
        public SysConfig SysConfig { get; set; }

        /// <summary>
        /// 定义上次登录
        /// </summary>
        public SavedLastLoginUser SavedLastLoginUser { get; set; }

        /// <summary>
        /// 定义上次操作
        /// </summary>
        public SaveUserOperator SaveUserOperator { get; set; }

        #endregion

        #region 命令

        public ICommand LoginCommand { get; private set; }

        #endregion

        #region 构造方法

        public LoginWindowViewModel()
        {
            Title = "CCT登录";

            CurrentUser = new User();
            SysConfig = ConfigHelper.ReadSysConfig();
            SavedLastLoginUser = SysConfig.SavedLastLoginUser;
            SaveUserOperator = SysConfig.SaveUserOperator;   

            if (SaveUserOperator.X1.ToLower() == "false")
            {
                IsReAccount = false;
                CurrentUser.RememberPwd = false;
            }
            else
            {
                IsReAccount = true;
                CurrentUser.RememberPwd = true;
            }

            if(SaveUserOperator.X2.ToLower()=="false")
            {
                IsAutoLogin = false;
                CurrentUser.AutomaticLogon= false;
            }
            else
            {
                IsAutoLogin = true;
                CurrentUser.AutomaticLogon = true;
            }

            //显示上次登录信息
            if (IsReAccount || IsAutoLogin)
            {
                UserName = SavedLastLoginUser.X1;
                UserPassword = SavedLastLoginUser.X2;
            }

            CurrentUser.UserName = UserName;
            CurrentUser.UserPassword = UserPassword;

            LoginCommand = new DelegateCommand<Window>(LoginCommandExecute);
        }

        #endregion

        #region 登录命令

        private void LoginCommandExecute(Window win)
        {
            int feedBack = Verification();//验证登录

            switch (feedBack)
            {
                case 1:
                    SaveLogin();
                    UpdateLoginDate();
                    //生成主窗体
                    MainWindow mainWindow = new MainWindow()
                    {
                        DataContext = new MainWindowViewModel(CurrentUser)
                    };
                    //设置系统主窗体
                    App.Current.MainWindow = mainWindow;
                    //关闭登陆界面
                    win.Close();
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
            try{
                if (string.IsNullOrEmpty(UserName))
                {
                    flag = -2;
                    return flag;
                }
                if (string.IsNullOrEmpty(UserPassword))
                {
                    flag = -3;
                    return flag;
                }
                User user = UserService.Login(new User() { UserName = UserName, UserPassword = UserPassword });//服务方法
                //验证登录
                if (user == null)
                {
                    flag = -4;
                }
                else
                {
                    flag = 1;
                    CurrentUser = user;
                    CurrentUser.RememberPwd = IsReAccount;
                    CurrentUser.AutomaticLogon = IsAutoLogin;
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
        public void SaveLogin()
        {
            SavedLastLoginUser.X1 = CurrentUser.UserName;
            SavedLastLoginUser.X2 = CurrentUser.UserPassword;
            SaveUserOperator.X1 = IsReAccount.ToString();
            SaveUserOperator.X2 = IsAutoLogin.ToString();
            SysConfig.SavedLastLoginUser = SavedLastLoginUser;
            ConfigHelper.SaveSysConfig(SysConfig);
        }

        #endregion

        #region 更新登录时间

        /// <summary>
        /// 更新登录信息到数据库
        /// </summary>
        public void UpdateLoginDate()
        {
            CurrentUser.UserLoginDate = DateTime.Now;
            UserService.UpdateUserLoginDate(CurrentUser);
        }

        #endregion

        #region 通知

        public void NotifyUI()
        {
            RaisePropertyChanged(nameof(CurrentUser));
            RaisePropertyChanged(nameof(UserName));
            RaisePropertyChanged(nameof(UserPassword));
        }

        #endregion
    }
}
