using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CCT.Resource.Helpers
{
    /// <summary>
    /// 编辑框相关处理帮助类
    /// </summary>
    public class RichTextBoxHelper
    {
        #region 文件加载
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="filename"></param>
        public static void LoadFile(string filename, RichTextBox richTextBox)
        {
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("文件不存在！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (!File.Exists(filename))
            {
                MessageBox.Show("文件不存在！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            using (FileStream stream = File.OpenRead(filename))
            {
                TextRange documentTextRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                string dataFormat = DataFormats.Text;
                StreamReader sr = new StreamReader(stream, Encoding.Default);
                documentTextRange.Load(new MemoryStream(Encoding.UTF8.GetBytes(sr.ReadToEnd())), dataFormat);
            }
        }
        #endregion

        #region 文件保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="richTextBox"></param>
        public static void SaveFile(string filename, RichTextBox richTextBox)
        {
            if (string.IsNullOrEmpty(filename))
            {
                MessageBox.Show("路径不存在！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            using (FileStream stream = File.OpenWrite(filename))
            {
                TextRange documentTextRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                string dataFormat = DataFormats.Text;
                string ext = System.IO.Path.GetExtension(filename);
                if (String.Compare(ext, ".xaml", true) == 0)
                {
                    dataFormat = DataFormats.Xaml;
                }
                else if (String.Compare(ext, ".rtf", true) == 0)
                {
                    dataFormat = DataFormats.Rtf;
                }
                documentTextRange.Save(stream, dataFormat);
            }
        }
        #endregion
    }
}
