using Microsoft.Win32;

namespace CCT.Resource.Helpers
{
    /// <summary>
    /// 文件对话框帮助类
    /// </summary>
    public class FileDialogHelper
    {
        #region OpenFileDialog

        /// <summary>
        /// 打开指定文件
        /// </summary>
        /// <returns></returns>
        public static string OpenFile(string ext)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "打开" + ext + "文件";//窗口标题
            dialog.DefaultExt = ext;//默认扩展名
            dialog.Filter = "." + ext + "文件|*." + ext;
            dialog.FileName = string.Empty;//默认文件名
            dialog.Multiselect = false;//不允许多选
            dialog.FilterIndex = 1;// 在对话框中选择的文件筛选器的索引，如果选第一项就设为1
            dialog.RestoreDirectory = true;//控制对话框在关闭之前是否恢复当前目录
            dialog.CheckPathExists = true;//检查路径是否存在
            dialog.CheckFileExists = true;//在对话框返回之前，检查指定路径是否存在
            dialog.AddExtension = true;//是否自动添加默认扩展名
            dialog.ValidateNames = true;//控制对话框检查文件名中是否不含有无效的字符或序列
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return string.Empty;
        }

        #endregion

        #region SaveFileDialog

        /// <summary>
        /// 另存为指定文件
        /// </summary>
        /// <returns></returns>
        public static string SaveAsFile(string ext)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "另存为" + ext + "文件";//窗口标题
            dialog.DefaultExt = ext;//默认扩展名
            dialog.Filter = "." + ext + "文件|*." + ext;
            dialog.FileName = string.Empty;//默认文件名
            dialog.FilterIndex = 1;// 在对话框中选择的文件筛选器的索引，如果选第一项就设为1
            dialog.RestoreDirectory = true;//控制对话框在关闭之前是否恢复当前目录
            //dialog.CheckPathExists = true;//检查路径是否存在
            //dialog.CheckFileExists = true;//在对话框返回之前，检查指定路径是否存在
            //dialog.AddExtension = true;//是否自动添加默认扩展名
            //dialog.ValidateNames = true;//控制对话框检查文件名中是否不含有无效的字符或序列
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return string.Empty;
        }

        #endregion
    }
}
