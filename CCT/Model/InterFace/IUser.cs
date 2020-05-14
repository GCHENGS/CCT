using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Model.InterFace
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        string UserPassword { get; set; }

        /// <summary>
        /// 用户电话
        /// </summary>
        string UserPhone { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        string UserHeadIcon { get; set; }

        /// <summary>
        /// 登录日期
        /// </summary>
        DateTime UserLoginDate { get; set; }

        /// <summary>
        /// 退出日期
        /// </summary>
        DateTime UserQuiteDate { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        DateTime UserCreateDate { get; set; }
    }
}
