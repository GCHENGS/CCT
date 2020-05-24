using CCT.Model.DataType;
using CCT.Model.Helper;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CCT.Service
{
    public class UserService
    {
        #region 登录

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        public static User Login(User user)
        {
            User ret = null;

            SqlConnection sqlCon = null;
            try
            {
                sqlCon = DBUtils.GetSqlConnection(); //获取连接
            }
            catch
            {
                return ret;
            }

            string commandText = "select * from Users where userName = '"+user.UserName+"' and userPassword = '"+user.UserPassword+"'";

            SqlParameter[] parameters = new SqlParameter[2]
            { 
                new SqlParameter("@userName", SqlDbType.Text),
                new SqlParameter("@userPassword", SqlDbType.Text)
            };

            parameters[0].Value = user.UserName;
            parameters[1].Value = user.UserPassword;

            SqlDataReader sr = DBUtils.ExecuteReader(sqlCon, commandText, CommandType.Text, parameters);//执行Sql命令
            if (sr != null)
            {
                ret = new User();
                while (sr.Read())
                {
                    ret.UserId= (int)sr.GetValue(0);
                    ret.UserName = sr.GetValue(1).ToString();
                    ret.UserPassword = sr.GetValue(2).ToString();
                    ret.UserPhone = sr.GetValue(3).ToString();
                    ret.UserHeadIcon = sr.GetValue(4).ToString();
                    ret.UserLoginDate = (DateTime)sr.GetValue(5);
                    ret.UserQuiteDate= (DateTime)sr.GetValue(6);
                    ret.UserCreateDate = (DateTime)sr.GetValue(7);
                }
            }
            sqlCon.Close();
            sr.Close();
            return ret;
        }

        #endregion

        #region 更新信息

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool UpdatePwd(string pwd,int id)
        {
            var ret = false;

            SqlConnection sqlCon = null;
            try
            {
                sqlCon = DBUtils.GetSqlConnection(); //获取连接
            }
            catch
            {
                return ret;
            }

            //string commandText = "update Users set userPassword = '"+ user.UserPassword +"' where userId = "+1;

            string commandText = "update Users set userPassword = @userPassword where userId = @userId";

            SqlParameter[] parameters = new SqlParameter[2]
            {
                    new SqlParameter("@userPassword", SqlDbType.Text),
                    new SqlParameter("@userId", SqlDbType.Int)
            };

            parameters[0].Value = pwd;

            parameters[1].Value = id;

            var n = DBUtils.ExecuteNonQuery(sqlCon, commandText, CommandType.Text, parameters);//执行Sql命令

            if(n==1)
            {
                ret = true;
            }

            sqlCon.Close();//关闭连接

            return ret;
        }


        /// <summary>
        /// 修改登录时间
        /// </summary>
        /// <returns></returns>
        public static bool UpdateUserLoginDate(User user)
        {
            var ret = false;

            SqlConnection sqlCon = null;
            try
            {
                sqlCon = DBUtils.GetSqlConnection(); //获取连接
            }
            catch
            {
                return ret;
            }

            //string commandText = "update Users set userLoginDate = '"+ user.UserLoginDate +"' where userId = "+1;

            string commandText = "update Users set userLoginDate = @userLoginDate where userId = @userId";

            SqlParameter[] parameters = new SqlParameter[2]
            {
                    new SqlParameter("@userLoginDate", SqlDbType.DateTime),
                    new SqlParameter("@userId", SqlDbType.Int)
            };

            parameters[0].Value = user.UserLoginDate;

            parameters[1].Value = user.UserId;

            var n = DBUtils.ExecuteNonQuery(sqlCon, commandText, CommandType.Text, parameters);//执行Sql命令

            if (n == 1)
            {
                ret = true;
            }

            sqlCon.Close();//关闭连接

            return ret;
        }

        /// <summary>
        /// 修改退出时间
        /// </summary>
        /// <returns></returns>
        public static bool UpdateUserQuiteDate(User user)
        {
            var ret = false;

            SqlConnection sqlCon = null;
            try
            {
                sqlCon = DBUtils.GetSqlConnection(); //获取连接
            }
            catch
            {
                return ret;
            }

            //string commandText = "update Users set userQuiteDate = '"+ user.UserQuiteDate +"' where userId = "+1;

            string commandText = "update Users set userQuiteDate = @userQuiteDate where userId = @userId";

            SqlParameter[] parameters = new SqlParameter[2]
            {
                    new SqlParameter("@userQuiteDate", SqlDbType.DateTime),
                    new SqlParameter("@userId", SqlDbType.Int)
            };

            parameters[0].Value = user.UserQuiteDate;

            parameters[1].Value = user.UserId;

            var n = DBUtils.ExecuteNonQuery(sqlCon, commandText, CommandType.Text, parameters);//执行Sql命令

            if (n == 1)
            {
                ret = true;
            }

            sqlCon.Close();//关闭连接

            return ret;
        }

        /// <summary>
        /// 修改电话
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool UpdateTel(string tel, int id)
        {
            var ret = false;

            SqlConnection sqlCon = null;
            try
            {
                sqlCon = DBUtils.GetSqlConnection(); //获取连接
            }
            catch
            {
                return ret;
            }

            //string commandText = "update Users set userPhone = '"+ user.UserPhone +"' where userId = "+1;

            string commandText = "update Users set userPhone = @userPhone where userId = @userId";

            SqlParameter[] parameters = new SqlParameter[2]
            {
                    new SqlParameter("@userPhone", SqlDbType.Text),
                    new SqlParameter("@userId", SqlDbType.Int)
            };

            parameters[0].Value = tel;

            parameters[1].Value = id;

            var n = DBUtils.ExecuteNonQuery(sqlCon, commandText, CommandType.Text, parameters);//执行Sql命令

            if (n == 1)
            {
                ret = true;
            }

            sqlCon.Close();//关闭连接

            return ret;
        }

        #endregion
    }
}
