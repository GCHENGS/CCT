using CCT.View;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace CCT.ViewModel
{
    /// <summary>
    /// 登录窗口模块
    /// </summary>
    public class LoginWindowViewModel : ViewModelBase
    {
        #region 局部变量
        /// <summary>
        /// 用户
        /// </summary>
        UserInfo user = null;
        #endregion

        #region 成员
        private string userName;
        private string userPassword;
        #endregion

        #region 属性
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { this.SetProperty(ref this.userName, value); }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string UserPassword
        {
            get { return userPassword; }
            set { this.SetProperty(ref this.userPassword, value); }
        }
        #endregion

        #region 命令声明 
        /// <summary>
        /// 
        /// </summary>
        public ICommand SwitchSkinCommand { get; set; }
        #endregion

        #region 构造方法
        public LoginWindowViewModel()
        {
            SwitchSkinCommand = new DelegateCommand(SwitchSkinCommandExecute);

            UserName = "yeshen";

            UserPassword = "111";

            user = new UserInfo()
            {
                UserName=userName,
                UserPwd=userPassword
            };
        }
        #endregion

        #region 命令执行

        private void SwitchSkinCommandExecute()
        {
            
        }

        #endregion

        #region 工作方法
        /// <summary>
        /// 用于验证登陆信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>
        /// 返回值代表 
        /// -6：密码不正确  -5：用户名不正确
        /// -4：密码含有特殊字符 -3：密码不能为空
        /// -2：用户名不能为空
        /// -1：数据库未连接 0：登陆失败
        ///  1：登陆成功  2：
        /// 
        /// </returns>
        public int Verification(ref int userID)
        {
            int flag = 0;
            try
            {
                if (string.IsNullOrEmpty(this.userName))
                {
                    //用户名不能为空
                    flag = -2;
                    return flag;
                }
                if (string.IsNullOrEmpty(this.userPassword))
                {
                    //密码不能为空
                    flag = -3;
                    return flag;
                }
                //正则表达式
                var r = new System.Text.RegularExpressions.Regex(@"\-");
                if (r.IsMatch(this.userPassword) || r.IsMatch(this.userName))
                {
                    //密码含有特殊字符
                    flag = -4;
                    return flag;
                }

                //验证登录
                if (this.userName != "sss")
                {
                    //用户名不正确
                    flag = -5;
                }
                else
                {
                    //var userInfo = dicUser.FirstOrDefault(u => u.Key == this.userName && u.Value == this.userPassword);

                    //if (userInfo.Key != null && userInfo.Value != null)
                    {
                        userID = 100;
                        //int id = userInfo.ID;
                        flag = 1;
                        //休眠1.5秒
                        System.Threading.Thread.Sleep(1500);
                    }
                    //else
                    {
                        //密码不正确
                        //flag = -6;
                    }
                }
            }
            catch (Exception)
            {
                //数据库连接失败
                flag = -1;
            }
            return flag;
        }

        #endregion
    }
}
