using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Word = Microsoft.Office.Interop.Word;

namespace CCT.Resource.Helpers
{
    /// <summary>
    /// word生成器
    /// </summary>
    public class WordGenerateHelper
    {

        #region 打开Word文档,并且返回对象wDoc,wDoc 
        /// 完整Word文件路径+名称  
        /// 返回的Word.Document wDoc对象 
        /// 返回的Word.Application对象 
        public static void CreateWordDocument(string FileName, ref Word.Document wDoc, ref Word.Application WApp)
        {
            if (FileName == "") return;
            Word.Document thisDocument = null;
            Word.FormFields formFields = null;
            Word.Application thisApplication = new Word.Application();
            thisApplication.Visible = true;
            thisApplication.Caption = "";
            thisApplication.Options.CheckSpellingAsYouType = false;
            thisApplication.Options.CheckGrammarAsYouType = false;

            object filename = FileName;
            object ConfirmConversions = false;
            object ReadOnly = true;
            object AddToRecentFiles = false;

            object PasswordDocument = System.Type.Missing;
            object PasswordTemplate = System.Type.Missing;
            object Revert = System.Type.Missing;
            object WritePasswordDocument = System.Type.Missing;
            object WritePasswordTemplate = System.Type.Missing;
            object Format = System.Type.Missing;
            object Encoding = System.Type.Missing;
            object Visible = System.Type.Missing;
            object OpenAndRepair = System.Type.Missing;
            object DocumentDirection = System.Type.Missing;
            object NoEncodingDialog = System.Type.Missing;
            object XMLTransform = System.Type.Missing;

            try
            {
                Word.Document wordDoc =
                thisApplication.Documents.Open(ref filename, ref ConfirmConversions,
                ref ReadOnly, ref AddToRecentFiles, ref PasswordDocument, ref PasswordTemplate,
                ref Revert, ref WritePasswordDocument, ref WritePasswordTemplate, ref Format,
                ref Encoding, ref Visible, ref OpenAndRepair, ref DocumentDirection,
                ref NoEncodingDialog, ref XMLTransform);

                thisDocument = wordDoc;
                wDoc = wordDoc;
                WApp = thisApplication;
                formFields = wordDoc.FormFields;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region  将richTextBox带格式的文本另存为word文档

        //先创建一个word文档，全选word文档中的数据，然后全选richTextBox中的数据并复制，粘贴到word文档中，最后保存word文档，并关闭文档
        public static void SaveAsWord(string fileName, RichTextBox richTextBox)
        {
            Word.Application app = new Word.Application();
            Word.Document doc = null;
            object missing = System.Reflection.Missing.Value;
            object File = fileName;
            try
            {
                doc = app.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                doc.ActiveWindow.Selection.WholeStory();//全选
                //richTextBox.SelectAll();
                //Clipboard.SetData(DataFormats.Rtf, richTextBox.Selection.Text);//复制RTF数据到剪贴板
                TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                Clipboard.SetData(DataFormats.Rtf, textRange.Text.Substring(0, textRange.Text.Length-2));//复制RTF数据到剪贴板
                doc.ActiveWindow.Selection.Paste();
                doc.SaveAs(ref File, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing);
            }
            finally
            {
                if (doc != null)
                {
                    doc.Close(ref missing, ref missing, ref missing);
                    doc = null;
                }
                if (app != null)
                {
                    app.Quit(ref missing, ref missing, ref missing);
                    app = null;
                }
            }
        }

        #endregion

        #region 用richTextBox打开带格式的word文档。

        //先打开word文档，全选其中的内容并保存的剪切板中，最后在richTextBox中粘贴数据，并关闭文档
        public static void OpenWord(string fileName, RichTextBox richTextBox)

        {

            Word.Application app = new Word.Application();

            Word.Document doc = null;

            object missing = System.Reflection.Missing.Value;

            object File = fileName;

            object readOnly = false;

            object isVisible = true;

            try

            {

                doc = app.Documents.Open(ref File, ref missing, ref readOnly,

                 ref missing, ref missing, ref missing, ref missing, ref missing,

                 ref missing, ref missing, ref missing, ref isVisible, ref missing,

                 ref missing, ref missing, ref missing);



                doc.ActiveWindow.Selection.WholeStory();//全选word文档中的数据

                doc.ActiveWindow.Selection.Copy();//复制数据到剪切板

                richTextBox.Paste();//richTextBox粘贴数据

                //richTextBox1.Text = doc.Content.Text;//显示无格式数据

            }

            finally

            {

                if (doc != null)

                {

                    doc.Close(ref missing, ref missing, ref missing);

                    doc = null;

                }



                if (app != null)

                {

                    app.Quit(ref missing, ref missing, ref missing);

                    app = null;

                }

            }

        }

        #endregion

        #region 保存richTextBox数据到已经存在的word文档中。

        //先打开word文档，全选word文档中的数据，然后全选richTextBox中的数据并复制，粘贴到word文档中，最后保存word文档，并关闭文档
        public static void SaveWord(string fileName, RichTextBox richTextBox)

        {

            Word.Application app = new Word.Application();

            Word.Document doc = null;

            object missing = System.Reflection.Missing.Value;

            object File = fileName;

            object readOnly = false;

            object isVisible = true;

            try

            {

                doc = app.Documents.Open(ref File, ref missing, ref readOnly,

                 ref missing, ref missing, ref missing, ref missing, ref missing,

                 ref missing, ref missing, ref missing, ref isVisible, ref missing,

                 ref missing, ref missing, ref missing);



                doc.ActiveWindow.Selection.WholeStory();//全选

                richTextBox.SelectAll();

                Clipboard.SetData(DataFormats.Rtf, richTextBox.Selection.Text);//复制RTF数据到剪贴板

                doc.ActiveWindow.Selection.Paste();

                //doc.Paragraphs.Last.Range.Text = richTextBox1.Text;//word文档赋值数据，不带格式

                doc.Save();

            }

            finally

            {

                if (doc != null)

                {

                    doc.Close(ref missing, ref missing, ref missing);

                    doc = null;

                }



                if (app != null)

                {

                    app.Quit(ref missing, ref missing, ref missing);

                    app = null;

                }

            }

        }

        #endregion
    }
}
