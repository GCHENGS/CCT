using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Model.DataType
{
    /// <summary>
    /// 加载的文件
    /// </summary>
    public class LoadedFile:BaseModel
    {
        #region 私有域
        private string fileName;//文件名

        private string filePath;//文件路径

        private string fileExt;//文件扩展名

        private bool isSave = true;//记录是否保存

        private bool isOpen = false;//记录是否打开

        private bool isEnabled = false;//标记复制,剪切,删除是否可用
        #endregion

        #region 属性
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { SetProperty(ref fileName, value); }
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set { SetProperty(ref filePath, value); }
        }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExt
        {
            get { return fileExt; }
            set { SetProperty(ref fileExt, value); }
        }

        /// <summary>
        /// 文件是否保存
        /// </summary>
        public bool IsSave
        {
            get { return isSave; }
            set { SetProperty(ref isSave, value); }
        }

        /// <summary>
        /// 文件是否打开
        /// </summary>
        public bool IsOpen
        {
            get { return isOpen; }
            set { SetProperty(ref isOpen, value); }
        }

        /// <summary>
        /// 标记复制,剪切,删除是否可用
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetProperty(ref isEnabled, value); }
        }
        #endregion
    }
}
