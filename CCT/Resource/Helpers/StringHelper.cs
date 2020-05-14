using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Resource.Helpers
{
    //处理字符串的帮助类
    public class StringHelper
    {
        #region 文件相关

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return string.Empty;
            return Path.GetFileName(filePath);
        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static string GetFileExt(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return string.Empty;
            var last = fileName.LastIndexOf(".");
            return fileName.Substring(last + 1, fileName.Length-last-1);
        }

        #endregion

    }
}
