using CCT.Model.Helper;
using System.Data;
using System.Data.SqlClient;

namespace CCT.Service
{
    public class UserService
    {

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
    }
}
