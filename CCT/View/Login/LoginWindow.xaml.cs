using CCT.Component.Exceptions;
using CCT.ViewModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace CCT.View
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        #region 构造函数
        public LoginWindow()
        {
            InitializeComponent();
            this.Loaded += LoginWindow_Loaded;
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
        }
        #endregion

        #region 局部变量

        LoginWindowViewModel _viewmodel;

        UserInfo user;

        /// <summary>
        /// 生成保存登录信息的XML文件
        /// 同时获取XML文件数据
        /// </summary>
        LoginInfoXmlHelper loginInfoXmlHelper = new LoginInfoXmlHelper();

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

        #region 界面事件

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink1_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://localhost:8080/CCT/regist");
        }

        /// <summary>
        /// 找回事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FoundLink_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegistLink_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 登录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.LoadGrid.Visibility = System.Windows.Visibility.Visible;//遮罩层可见
                bw.RunWorkerAsync();
            }
            catch (Exception)
            {
                //报错无视
            }
        }

        /// <summary>
        /// 登陆后台运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //int userID = 0;
            //反馈
            //feedBack = _viewmodel.Verification(ref userID);
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
                    //获取新的登录信息
                    //var userInfo = new UserInfo
                    //{
                    //    AutomaticLogon = (bool)this.checkBox2.IsChecked,
                    //    RememberPwd = (bool)this.checkBox1.IsChecked,
                    //    UserName = this.cmb1.Text.Trim(),
                    //    UserPwd = this.passwordBox1.Password.Trim(),
                    //};
                    //if (!(bool)this.checkBox1.IsChecked)
                    //{
                    //    userInfo.UserPwd = "";
                    //}
                    //listUserInfo.Remove(listUserInfo.FirstOrDefault(u => u.UserName == userInfo.UserName));
                    //listUserInfo.Insert(0, userInfo);
                    //保存登录信息
                    //loginInfoXmlHelper.CreateXml(listUserInfo.ToList());
                    //生成主窗体
                    MainWindow mainWindow = new MainWindow();
                    //设置系统主窗体
                    App.Current.MainWindow = mainWindow;
                    //关闭登陆界面
                    this.Close();
                    //this.DialogResult = true;
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
                    System.Windows.MessageBox.Show("密码含有特殊字符！", "系统提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    break;
                case -5:
                    System.Windows.MessageBox.Show("用户名不正确！", "系统提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    break;
                case -6:
                    System.Windows.MessageBox.Show("密码不正确！", "系统提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    break;
                default:
                    MessageBox.Show("未知错误！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
            this.LoadGrid.Visibility = System.Windows.Visibility.Collapsed;//遮罩层隐藏
        }


        /// <summary>
        /// 窗体加载完成后操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (_viewmodel == null)
            {
                //界面中浮云移动动画
                Storyboard sbd = Resources["sbCloud"] as Storyboard;
                sbd.Begin();

                _viewmodel = new LoginWindowViewModel();
                this.DataContext = _viewmodel;

                //获取登录信息
                user = loginInfoXmlHelper.GetLoginInfo();

                //计时器 为登录框添加数据计时执行
                var timer1 = new System.Timers.Timer
                {
                    Interval = 100,
                };
                timer1.Elapsed += new System.Timers.ElapsedEventHandler(theout1);//到达时间的时候执行事件；
                timer1.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；   
                timer1.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
                //计时器 自动登录做准备的数据计时执行
                var timer2 = new System.Timers.Timer
                {
                    Interval = 400,
                };
                timer2.Elapsed += new System.Timers.ElapsedEventHandler(theout2);
                timer2.AutoReset = false;
                timer2.Enabled = true;
            }
        }

        /// <summary>
        /// 为登录框添加数据计时执行
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void theout1(object source, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                //显示上次登录
                //this.cmb1.SelectedIndex = 0;
            }));
        }
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
                var userLogin = user;
                if (userLogin != null)
                {
                    if (userLogin.AutomaticLogon)//自动登录
                    {
                        //触发登录
                        this.LoginButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, this.LoginButton));
                    }
                }
            }));
        }

        /// <summary>
        /// 记录UserInfo信息
        /// </summary>
        /// <param name="userinfo"></param>
        private void UserInfoRecond(UserInfo userinfo)
        {
            this.ReAccount.RaiseEvent(new RoutedEventArgs(CheckBox.UncheckedEvent, this.ReAccount));

            if (userinfo.AutomaticLogon)
            {
                this.AutoLogin.RaiseEvent(new RoutedEventArgs(CheckBox.CheckedEvent, this.AutoLogin));
            }
            else if (!userinfo.RememberPwd)
            {
                this.ReAccount.RaiseEvent(new RoutedEventArgs(CheckBox.UncheckedEvent, this.ReAccount));
            }
            else if (userinfo.RememberPwd)
            {
                this.ReAccount.RaiseEvent(new RoutedEventArgs(CheckBox.CheckedEvent, this.ReAccount));
            }
        }

        #endregion

        #region 保存帐号

        /// <summary>
        /// 保存账号(异步执行)
        /// </summary>
        /// <param name="loginName"></param>
        //private async void SaveLoginName(string loginName)
        //{
            //if (this.SaveAccounts.IsChecked != true) return;
            //try
            //{
            //    string filePath = OfficeOpenXml.Utils.GetMapPath("") + @"Config\SysConfig.xml";
            //    XmlDocument doc = XmlHelper. .GetXMLDoc(filePath);
            //    if (doc != null)
            //    {
            //        XmlNode rootNode = doc.SelectSingleNode("SysConfig");
            //        List<string> nameList = XmlHelper.GetNodesToList(rootNode, "SavedLoginNames");
            //        if (nameList != null && !nameList.Contains(loginName))// 如果账号仍未保存
            //        {
            //            XmlNode namesNode = rootNode.SelectSingleNode("SavedLoginNames");// 获取SavedLoginNames节点信息
            //            XmlNode childNode = doc.CreateElement("LoginName");// 创建新的SavedLoginName节点
            //            childNode.InnerText = loginName;// 节点内容值为loginName参数
            //            namesNode.PrependChild(childNode);// 插入到最前
 
            //            namesNode = XmlHelper.HandleNodeUnderMaxCount(namesNode, maxCount);// 处理保存账号的最大数量大于config表中配置的数量
            //            doc.Save(filePath);// 保存
            //        }
            //        else if (nameList != null && nameList.Contains(loginName))// 如果账号已保存, 将其提到最前。
            //        {
            //            XmlNode namesNode = rootNode.SelectSingleNode("SavedLoginNames");
            //            XmlNodeList childNodes = namesNode.ChildNodes;
            //            foreach (XmlNode item in childNodes)
            //            {
            //                if (loginName.Equals(item.InnerText))
            //                {
            //                    XmlNode newNode = item;
            //                    namesNode.RemoveChild(item);
            //                    namesNode.PrependChild(newNode);
            //                    break;
            //                }
            //            }
            //            doc.Save(filePath);// 保存
            //        }
            //    }
            //}
            //catch (ReadSysConfigFailedException ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
       // }

        #endregion

        #region Load登录加载界面

        #region 登陆加载界面局部变量
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
        #endregion

        #endregion
    }

    #region 用户类
    /// <summary>
    /// 用户类用于保存记录
    /// </summary>
    public class UserInfo
    {
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        /// <summary>
        /// 是否自动登录
        /// </summary>
        [System.ComponentModel.DefaultValue(false)]
        public bool AutomaticLogon { get; set; }
        /// <summary>
        /// 是否记住密码
        /// </summary>
        [System.ComponentModel.DefaultValue(false)]
        public bool RememberPwd { get; set; }
    }
    #endregion

    #region 加解密
    /// <summary>
    /// 加密类 
    /// </summary>
    internal class CryptInfo
    {
        /// <summary>
        /// 私有构造方法禁止实例化该类型
        /// </summary>
        public CryptInfo() { }

        /// <summary>
        /// 对输入的字符串进行加密，并获取加密后的字符串
        /// </summary>
        /// <param name="text">输入的字符串</param>
        /// <returns></returns>
        public string GetEncrypt(string text)
        {
            return this.Encrypt(text);
        }

        /// <summary>
        /// 加密 对用户名和密码进行加密的方法
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string Encrypt(string text)
        {
            Rijndael crypt = Rijndael.Create();
            byte[] key = new byte[32] { 0XA6, 0X7D, 0XE1, 0X3F, 0X35, 0X0E, 0XE1, 0XA9, 0X83, 0XA5, 0X62, 0XAA, 0X7A, 0XAE, 0X79, 0X98, 0XA7, 0X33, 0X49, 0XFF, 0XE6, 0XAE, 0XBF, 0X8D, 0X8D, 0X20, 0X8A, 0X49, 0X31, 0X3A, 0X12, 0X40 };

            byte[] iv = new byte[16] { 0XF8, 0X8B, 0X01, 0XFB, 0X08, 0X85, 0X9A, 0XA4, 0XBE, 0X45, 0X28, 0X56, 0X03, 0X42, 0XF6, 0X19 };
            crypt.Key = key;
            crypt.IV = iv;

            MemoryStream ms = new MemoryStream();

            ICryptoTransform transtormEncode = new ToBase64Transform();
            //Base64编码
            CryptoStream csEncode = new CryptoStream(ms, transtormEncode, CryptoStreamMode.Write);

            CryptoStream csEncrypt = new CryptoStream(csEncode, crypt.CreateEncryptor(), CryptoStreamMode.Write);

            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] rawData = enc.GetBytes(text);

            csEncrypt.Write(rawData, 0, rawData.Length);
            csEncrypt.FlushFinalBlock();

            byte[] encryptedData = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(encryptedData, 0, (int)ms.Length);

            return enc.GetString(encryptedData);
        }

    }

    /// <summary>
    /// 解密类
    /// </summary>
    /// 
    internal class DecryptInfo
    {
        /// <summary>
        /// 私有化构造函数不允许外部实例化
        /// </summary>
        public DecryptInfo()
        { }


        /// <summary>
        /// 对输入的字符串进行加密,并获取解密后的字符串
        /// </summary>
        /// <param name="text">输入的字符串</param>
        /// <returns></returns>
        public string GetDecrypte(string text)
        {
            return this.Decrypt(text);
        }

        /// <summary>
        /// 解密方法 对用户名和密码进行解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string Decrypt(string text)
        {
            Rijndael crypt = Rijndael.Create();
            byte[] key = new byte[32] { 0XA6, 0X7D, 0XE1, 0X3F, 0X35, 0X0E, 0XE1, 0XA9, 0X83, 0XA5, 0X62, 0XAA, 0X7A, 0XAE, 0X79, 0X98, 0XA7, 0X33, 0X49, 0XFF, 0XE6, 0XAE, 0XBF, 0X8D, 0X8D, 0X20, 0X8A, 0X49, 0X31, 0X3A, 0X12, 0X40 };
            byte[] iv = new byte[16] { 0XF8, 0X8B, 0X01, 0XFB, 0X08, 0X85, 0X9A, 0XA4, 0XBE, 0X45, 0X28, 0X56, 0X03, 0X42, 0XF6, 0X19 };
            crypt.Key = key;
            crypt.IV = iv;

            MemoryStream ms = new MemoryStream();
            CryptoStream csDecrypt = new CryptoStream(ms, crypt.CreateDecryptor(), CryptoStreamMode.Write);
            ICryptoTransform transformDecode = new FromBase64Transform();
            CryptoStream csDecode = new CryptoStream(csDecrypt, transformDecode, CryptoStreamMode.Write);

            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] rawData = enc.GetBytes(text);
            csDecode.Write(rawData, 0, rawData.Length);
            csDecode.FlushFinalBlock();

            byte[] decryptedData = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(decryptedData, 0, (int)ms.Length);

            return (enc.GetString(decryptedData));
        }
    }
    #endregion

    #region LoginInfoXmlHelper
    /// <summary>
    /// Xml配置文件
    /// </summary>
    public class LoginInfoXmlHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        public LoginInfoXmlHelper()
        {
            string appStartPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            StringBuilder builder = new StringBuilder();
            builder.Append(appStartPath);
            builder.Append("\\AthenFile\\");
            builder.Append("\\XML\\");
            System.IO.DirectoryInfo directoryInfo = new DirectoryInfo(builder.ToString());
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();//创建一个
            }
            builder.Append("LoginInfoXml.xml");
            this.fullFilePath = builder.ToString();

            CreateXml(new UserInfo()
            {
                UserName = "yeshen",
                UserPwd = "111"
            });
        }

        #region 成员
        /// <summary>
        /// 文件全路径
        /// </summary>
        string fullFilePath;
        /// <summary>
        /// 加密
        /// </summary>
        CryptInfo cryptInfo = new CryptInfo();
        /// <summary>
        /// 解密
        /// </summary>
        DecryptInfo decryptInfo = new DecryptInfo();

        #endregion

        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <returns></returns>
        public UserInfo GetLoginInfo()
        {
            var loginInfo = new UserInfo();
            if (InfoFile.Exists)
            {
                XmlSerializer xml = new XmlSerializer(loginInfo.GetType());
                using (Stream s = InfoFile.OpenRead())
                {
                    loginInfo = xml.Deserialize(s) as UserInfo;
                }
            }

            var tList = new UserInfo()
            {
                AutomaticLogon = loginInfo.AutomaticLogon,
                RememberPwd = loginInfo.RememberPwd,
                UserName = this.decryptInfo.GetDecrypte(loginInfo.UserName),
                UserPwd = this.decryptInfo.GetDecrypte(loginInfo.UserPwd),
            };

            return tList;
        }

        /// <summary>
        /// 文件方式
        /// </summary>
        private FileInfo InfoFile
        {
            get
            {
                return new FileInfo(fullFilePath);
            }
        }

        /// <summary>
        /// 生成Xml文件
        /// </summary>
        /// <param name="loginInfo"></param>
        private void CreateXml(UserInfo loginInfo)
        {
            var tList = new UserInfo()
            {
                AutomaticLogon = loginInfo.AutomaticLogon,
                RememberPwd = loginInfo.RememberPwd,
                UserName = this.cryptInfo.GetEncrypt(loginInfo.UserName),
                UserPwd = this.cryptInfo.GetEncrypt(loginInfo.UserPwd),
            };

            XmlSerializer xmls = new XmlSerializer(tList.GetType());

            if (InfoFile.Exists)
            {
                InfoFile.Delete();
            }

            using (Stream s = InfoFile.OpenWrite())
            {
                xmls.Serialize(s, tList);
                s.Close();
            }
        }
    }
    #endregion
}
