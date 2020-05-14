using CCT.Model.InterFace;
using System;

namespace CCT.Model.DataType
{
    /// <summary>
    /// 用户模型
    /// </summary>
    public class User : BaseModel,IUser
    {
        #region 私有域
        private int userId;
        private string userName;
        private string userPassword;
        private string userPhone;
        private string userHeadIcon;
        private DateTime userLoginDate;
        private DateTime userQuiteDate;
        private DateTime userCreateDate;
        #endregion

        #region 属性

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId
        {
            get { return userId; }
            set { SetProperty(ref userId, value); }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string UserPassword
        {
            get { return userPassword; }
            set { SetProperty(ref userPassword, value); }
        }

        /// <summary>
        /// 用户电话
        /// </summary>
        public string UserPhone
        {
            get { return userPhone; }
            set { SetProperty(ref userPhone, value); }
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserHeadIcon
        {
            get { return userHeadIcon; }
            set { SetProperty(ref userHeadIcon, value); }
        }

        /// <summary>
        /// 登录日期
        /// </summary>
        public DateTime UserLoginDate
        {
            get { return userLoginDate; }
            set { SetProperty(ref userLoginDate, value); }
        }

        /// <summary>
        /// 退出日期
        /// </summary>
        public DateTime UserQuiteDate
        {
            get { return userQuiteDate; }
            set { SetProperty(ref userQuiteDate, value); }
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime UserCreateDate
        {
            get { return userCreateDate; }
            set { SetProperty(ref userCreateDate, value); }
        }

        #endregion

        #region 构造方法

        /// <summary>
        /// 用作登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        public User(string userName,string userPassword)
        {
            UserName = userName;
            UserPassword = userPassword;
        }

        public User()
        {

        }

        #endregion
    }
}
