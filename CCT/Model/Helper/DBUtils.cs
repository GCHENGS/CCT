using CCT.Resource.Helpers.FileHelper;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CCT.Model.Helper
{
    /// <summary>
    /// //数据库工具
    /// </summary>
    public class DBUtils
    {
        /// <summary>
        /// 属性文件帮助类
        /// </summary>
        private static PropertiesHelper po = new PropertiesHelper("D:/VS开发/CCT/CCT/Service/DB/db.properties");

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection() //连接数据库
            {
                ConnectionString = "Data Source = "+ po["DataSource"].ToString()
                + ";Initial Catalog ="+ po["InitialCatalog"].ToString() 
                + ";User Id="+ po["UserId"].ToString() 
                + ";Password=" + po["Password"].ToString()
            };
        }

        /// <summary>
        /// 获取sql命令
        /// </summary>
        /// <returns></returns>
        public static SqlCommand GetSqlCommand(string sql, SqlConnection sqlCon)
        {
            return new SqlCommand(sql,sqlCon);
        }

        // Set the connection, command, and then execute the command with non query.  
        public static int ExecuteNonQuery(SqlConnection conn, string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            SqlCommand cmd = GetSqlCommand(commandText, conn);
            cmd.CommandType = commandType;
            cmd.Parameters.AddRange(parameters);
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch
            {
                return -1;
            }    
        }

        // Set the connection, command, and then execute the command and only return one value.  
        public static object ExecuteScalar(SqlConnection conn, string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            SqlCommand cmd = GetSqlCommand(commandText, conn);
            cmd.CommandType = commandType;
            cmd.Parameters.AddRange(parameters);
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch
            {
                return null;
            }
        }

        // Set the connection, command, and then execute the command with query and return the reader.  
        public static SqlDataReader ExecuteReader(SqlConnection conn , string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            SqlCommand cmd = GetSqlCommand(commandText, conn);
            //cmd.CommandType = commandType;
            //cmd.Parameters.AddRange(parameters);
            try
            {
                conn.Open(); 
                return cmd.ExecuteReader();
            }
            catch
            {
                return null;
            } 
        }
    }
}
