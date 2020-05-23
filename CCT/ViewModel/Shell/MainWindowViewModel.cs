using CCT.Config;
using CCT.Model.DataType;
using CCT.Resource.Enums;
using CCT.Resource.Helpers;
using CCT.Service;
using CCT.View;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CCT.ViewModel
{
    /// <summary>
    /// 主窗体对应ViewModel
    /// </summary>
    public class MainWindowViewModel:ViewModelBase
    {
        #region 私有域

        private User currentUser;//当前用户

        private LoadedFile currentFile;//当前文件

        private ViewModelBase currentViewModel;//当前视图模型

        #endregion

        #region  属性

        /// <summary>
        /// 数据源
        /// </summary>
        public DataList DataList { get; set; }

        /// <summary>
        /// 数据集
        /// </summary>
        public DataSet DataSet { get; set; }

        /// <summary>
        /// 定义系统配置
        /// </summary>
        public SysConfig SysConfig { get; set; }

        /// <summary>
        /// 定义上次打开
        /// </summary>
        public SavedLastOpenFile SavedLastOpenFile { get; set; }

        /// <summary>
        /// 定义最近文件
        /// </summary>
        public List<RecentFile> List { get; set; }

        /// <summary>
        /// 当前用户
        /// </summary>
        public User CurrentUser
        {
            get { return currentUser; }
            set { SetProperty(ref currentUser, value); }
        }

        /// <summary>
        /// 当前文件
        /// </summary>
        public LoadedFile CurrentFile
        {
            get { return currentFile; }
            set { SetProperty(ref currentFile, value); }
        }

        /// <summary>
        /// 视图模型
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }

        #endregion

        #region 命令

        public ICommand NewXmlCommand { get; private set; }//新建XML文件
        public ICommand NewJsonCommand { get; private set; }//新建Json文件
        public ICommand NewInICommand { get; private set; }//新建InI文件
        public ICommand NewPropertiesCommand { get; private set; }//新建Properties文件

        public ICommand OpenXmlCommand { get; private set; }//打开XML文件
        public ICommand OpenJsonCommand { get; private set; }//打开Json文件
        public ICommand OpenInICommand { get; private set; }//打开InI文件
        public ICommand OpenPropertiesCommand { get; private set; }//打开Properties文件

        public ICommand SaveCommand { get; private set; }//保存
        public ICommand SaveAsCommand { get; private set; }//另存为

        public ICommand LookUserCommand { get; private set; }//查看帐户
        public ICommand UpdatePwdCommand { get; private set; }//修改密码
        public ICommand UpdateTelCommand { get; private set; }//修改电话

        public ICommand SaveExitCommand { get; private set; }//保存退出
        public ICommand ExitCommand { get; private set; }//不保存退出

        public ICommand UndoCommand { get; private set; }//撤销
        public ICommand RedoCommand { get; private set; }//重做
        public ICommand CutCommand { get; private set; }//剪切
        public ICommand CopyCommand { get; private set; }//复制
        public ICommand PasteCommand { get; private set; }//粘贴
        public ICommand DelCommand { get; private set; }//删除
        public ICommand SelAllCommand { get; private set; }//全选

        public ICommand ParseCommand { get; private set; }//解析

        public ICommand ExportTCommand { get; private set; }//导出txt
        public ICommand ExportWCommand { get; private set; }//导出word
        public ICommand ExportPCommand { get; private set; }//导出picture

        public ICommand RecentOpenCommand { get; private set; }//打开上次
        public ICommand RecentFileCommand { get; private set; }//打开最近

        public ICommand RichTextBoxTextChangedCommand { get; private set; }//文本变化事件
        public ICommand RichTextBoxSelectionChangedCommand { get; private set; }//选中文本变化事件

        #endregion

        #region 构造方法

        public MainWindowViewModel(User user)
        {
            CurrentUser = user;

            SysConfig = ConfigHelper.ReadSysConfig();
            SavedLastOpenFile = SysConfig.SavedLastOpenFile;
            List = SysConfig.SavedRecentFile.List;

            NewXmlCommand = new DelegateCommand<RichTextBox>(NewXmlCommandExecute);
            NewJsonCommand = new DelegateCommand<RichTextBox>(NewJsonCommandExecute);
            NewInICommand = new DelegateCommand<RichTextBox>(NewInICommandExecute);
            NewPropertiesCommand = new DelegateCommand<RichTextBox>(NewPropertiesCommandExecute);

            OpenXmlCommand = new DelegateCommand<RichTextBox>(OpenXmlCommandExecute);
            OpenJsonCommand = new DelegateCommand<RichTextBox>(OpenJsonCommandExecute);
            OpenInICommand = new DelegateCommand<RichTextBox>(OpenInICommandExecute);
            OpenPropertiesCommand = new DelegateCommand<RichTextBox>(OpenPropertiesCommandExecute);

            SaveCommand = new DelegateCommand<RichTextBox>(SaveCommandExecute);
            SaveAsCommand = new DelegateCommand<RichTextBox>(SaveAsCommandExecute);
            SaveExitCommand = new DelegateCommand<RichTextBox>(SaveExitCommandExecute);

            UndoCommand = new DelegateCommand<RichTextBox>(UndoCommandExecute);
            RedoCommand = new DelegateCommand<RichTextBox>(RedoCommandExecute);
            CutCommand = new DelegateCommand<RichTextBox>(CutCommandExecute);
            CopyCommand = new DelegateCommand<RichTextBox>(CopyCommandExecute);
            PasteCommand = new DelegateCommand<RichTextBox>(PasteCommandExecute);
            DelCommand = new DelegateCommand<RichTextBox>(DelCommandExecute);
            SelAllCommand = new DelegateCommand<RichTextBox>(SelAllCommandExecute);

            LookUserCommand = new DelegateCommand<Window>(LookUserCommandExecute);
            UpdatePwdCommand = new DelegateCommand<Window>(UpdatePwdCommandExecute);
            UpdateTelCommand = new DelegateCommand<Window>(UpdateTelCommandExecute);
            ExitCommand = new DelegateCommand<Window>(ExitCommandExecute);

            ParseCommand = new DelegateCommand<Window>(ParseCommandExecute);

            ExportTCommand = new DelegateCommand<RichTextBox>(ExportTCommandExecute);
            ExportWCommand = new DelegateCommand<RichTextBox>(ExportWCommandExecute);
            ExportPCommand = new DelegateCommand<RichTextBox>(ExportPCommandExecute);

            RecentOpenCommand = new DelegateCommand<RichTextBox>(RecentOpenCommandExecute);
            RecentFileCommand = new DelegateCommand<MenuItem>(RecentFileCommandExecute);

            RichTextBoxTextChangedCommand = new DelegateCommand<RichTextBox>(RichTextBoxTextChangedCommandExecute);
            RichTextBoxSelectionChangedCommand = new DelegateCommand<RichTextBox>(RichTextBoxSelectionChangedCommandExecute);
        }

        public MainWindowViewModel()
        {
            CurrentUser = new User();

            SysConfig = ConfigHelper.ReadSysConfig();
            SavedLastOpenFile = SysConfig.SavedLastOpenFile;
            List = SysConfig.SavedRecentFile.List;

            NewXmlCommand = new DelegateCommand<RichTextBox>(NewXmlCommandExecute);
            NewJsonCommand = new DelegateCommand<RichTextBox>(NewJsonCommandExecute);
            NewInICommand = new DelegateCommand<RichTextBox>(NewInICommandExecute);
            NewPropertiesCommand = new DelegateCommand<RichTextBox>(NewPropertiesCommandExecute);

            OpenXmlCommand = new DelegateCommand<RichTextBox>(OpenXmlCommandExecute);
            OpenJsonCommand = new DelegateCommand<RichTextBox>(OpenJsonCommandExecute);
            OpenInICommand = new DelegateCommand<RichTextBox>(OpenInICommandExecute);
            OpenPropertiesCommand = new DelegateCommand<RichTextBox>(OpenPropertiesCommandExecute);

            SaveCommand = new DelegateCommand<RichTextBox>(SaveCommandExecute);
            SaveAsCommand = new DelegateCommand<RichTextBox>(SaveAsCommandExecute);
            SaveExitCommand = new DelegateCommand<RichTextBox>(SaveExitCommandExecute);

            UndoCommand = new DelegateCommand<RichTextBox>(UndoCommandExecute);
            RedoCommand = new DelegateCommand<RichTextBox>(RedoCommandExecute);
            CutCommand = new DelegateCommand<RichTextBox>(CutCommandExecute);
            CopyCommand = new DelegateCommand<RichTextBox>(CopyCommandExecute);
            PasteCommand = new DelegateCommand<RichTextBox>(PasteCommandExecute);
            DelCommand = new DelegateCommand<RichTextBox>(DelCommandExecute);
            SelAllCommand = new DelegateCommand<RichTextBox>(SelAllCommandExecute);

            LookUserCommand = new DelegateCommand<Window>(LookUserCommandExecute);
            UpdatePwdCommand = new DelegateCommand<Window>(UpdatePwdCommandExecute);
            UpdateTelCommand = new DelegateCommand<Window>(UpdateTelCommandExecute);
            ExitCommand = new DelegateCommand<Window>(ExitCommandExecute);

            ParseCommand = new DelegateCommand<Window>(ParseCommandExecute);

            ExportTCommand = new DelegateCommand<RichTextBox>(ExportTCommandExecute);
            ExportWCommand = new DelegateCommand<RichTextBox>(ExportWCommandExecute);
            ExportPCommand = new DelegateCommand<RichTextBox>(ExportPCommandExecute);

            RecentOpenCommand = new DelegateCommand<RichTextBox>(RecentOpenCommandExecute);
            RecentFileCommand = new DelegateCommand<MenuItem>(RecentFileCommandExecute);

            RichTextBoxTextChangedCommand = new DelegateCommand<RichTextBox>(RichTextBoxTextChangedCommandExecute);
            RichTextBoxSelectionChangedCommand = new DelegateCommand<RichTextBox>(RichTextBoxSelectionChangedCommandExecute);
        }

        #endregion

        #region 新建

        /// <summary>
        /// 新建XML文件
        /// </summary>
        private void NewXmlCommandExecute(RichTextBox richTextBox)
        {
            if (richTextBox == null) return;

            //初始化当前文件类
            CurrentFile = new LoadedFile();
            currentFile.FileName = "未命名";
            currentFile.FileExt = "xml";
            currentFile.IsSave = false;
            currentFile.IsOpen = true;
            Title = "*" + currentFile.FileName + "-CCT通用配置工具";

            // 添加默认文本
            string myText = "<?xml version='1.0' encoding='UTF-8'?>\r\n";
            FlowDocument doc = new FlowDocument();
            Paragraph p = new Paragraph();
            Run r = new Run(myText);
            p.Inlines.Add(r);//Run级元素添加到Paragraph元素的Inline
            doc.Blocks.Add(p);//Paragraph级元素添加到流文档的块级元素
            richTextBox.Document = doc;

            //CurrentViewModel = new MainXmlViewModel();
        }

        /// <summary>
        /// 新建JSON文件
        /// </summary>
        private void NewJsonCommandExecute(RichTextBox richTextBox)
        {
            if (richTextBox == null) return;

            //初始化当前文件类
            CurrentFile = new LoadedFile();
            currentFile.FileName = "未命名";
            currentFile.FileExt = "json";
            currentFile.IsSave = false;
            currentFile.IsOpen = true;
            Title = "*" + currentFile.FileName + "-CCT通用配置工具";
            // 添加默认文本
            string myText = "/*对象: {key1：value1, key2：value2, ...}\r\n  数组:[\"java\", \"javascript\", \"vb\", ...]\r\n*/";
            FlowDocument doc = new FlowDocument();
            Paragraph p = new Paragraph();
            Run r = new Run(myText);
            p.Inlines.Add(r);//Run级元素添加到Paragraph元素的Inline
            doc.Blocks.Add(p);//Paragraph级元素添加到流文档的块级元素
            richTextBox.Document = doc;

            //CurrentViewModel = new MainXmlViewModel();
        }

        /// <summary>
        /// 新建INI文件
        /// </summary>
        private void NewInICommandExecute(RichTextBox richTextBox)
        {
            if (richTextBox == null) return;

            //初始化当前文件类
            CurrentFile = new LoadedFile();
            currentFile.FileName = "未命名";
            currentFile.FileExt = "ini";
            currentFile.IsSave = false;
            currentFile.IsOpen = true;
            Title = "*" + currentFile.FileName + "-CCT通用配置工具";

            // 添加默认文本
            string myText = ";INI文件由节、键、值组成.\r\n;[section(节)]\r\n;key(键)=value(值)\r\n";
            FlowDocument doc = new FlowDocument();
            Paragraph p = new Paragraph();
            Run r = new Run(myText);
            p.Inlines.Add(r);//Run级元素添加到Paragraph元素的Inline
            doc.Blocks.Add(p);//Paragraph级元素添加到流文档的块级元素
            richTextBox.Document = doc;

            //CurrentViewModel = new MainXmlViewModel();
        }

        /// <summary>
        /// 新建Properties文件
        /// </summary>
        private void NewPropertiesCommandExecute(RichTextBox richTextBox)
        {
            if (richTextBox == null) return;

            //初始化当前文件类
            CurrentFile = new LoadedFile();
            currentFile.FileName = "未命名";
            currentFile.FileExt = "properties";
            currentFile.IsSave = false;
            currentFile.IsOpen = true;
            Title = "*" + currentFile.FileName + "-CCT通用配置工具";

            // 添加默认文本
            string myText = "#Properties文件由键、值组成.\r\n#key(键)=value(值)\r\n";
            FlowDocument doc = new FlowDocument();
            Paragraph p = new Paragraph();
            Run r = new Run(myText);
            p.Inlines.Add(r);//Run级元素添加到Paragraph元素的Inline
            doc.Blocks.Add(p);//Paragraph级元素添加到流文档的块级元素
            richTextBox.Document = doc;

            //CurrentViewModel = new MainXmlViewModel();
        }

        #endregion

        #region 打开

        /// <summary>
        /// 打开XML文件
        /// </summary>
        private void OpenXmlCommandExecute(RichTextBox richTextBox)
        {
            OpenFile("xml", richTextBox);
        }

        /// <summary>
        /// 打开JSON文件
        /// </summary>
        private void OpenJsonCommandExecute(RichTextBox richTextBox)
        {
            OpenFile("json", richTextBox);
        }

        /// <summary>
        /// 打开INI文件
        /// </summary>
        private void OpenInICommandExecute(RichTextBox richTextBox)
        {
            OpenFile("ini", richTextBox);
        }

        /// <summary>
        /// 打开Properties文件
        /// </summary>
        private void OpenPropertiesCommandExecute(RichTextBox richTextBox)
        {
            OpenFile("properties",richTextBox);
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存文件
        /// </summary>
        private void SaveCommandExecute(RichTextBox richTextBox)
        {
            // 当前文件未初始化时，不能保存
            if (currentFile == null) return;

            // 判断保存路径是否存在
            if (!string.IsNullOrWhiteSpace(currentFile.FilePath))
            {
                // 调用保存指定xml地址的文件
                RichTextBoxHelper.SaveFile(currentFile.FilePath, richTextBox);

                // 初始化加载文件属性
                if(!currentFile.IsOpen) currentFile.IsOpen = true;
                if(!currentFile.IsSave) currentFile.IsSave = true;

                // 把标题改为保存文件的名称
                Title = currentFile.FileName + "-CCT通用配置工具";
                Status = currentFile.FilePath;
            }
            else
            {
                // 文件路径为空，说明新建文件第一次保存，执行“另存为”操作
                SaveAsCommandExecute(richTextBox);
            }
        }

        #endregion

        #region 另存
        /// <summary>
        /// 另存为文件
        /// </summary>
        private void SaveAsCommandExecute(RichTextBox richTextBox)
        {
            // 当前文件未初始化时，不能另存为
            if (currentFile == null) return;

            if (!string.IsNullOrWhiteSpace(currentFile.FilePath))
            {
                // 获取另存为路径
                var path = FileDialogHelper.SaveAsFile(currentFile.FileExt);

                if (!string.IsNullOrWhiteSpace(path))
                {
                    // 调用另存为指定xml地址的文件
                    RichTextBoxHelper.SaveFile(path, richTextBox);

                    // 初始化加载文件属性
                    if (!currentFile.IsOpen) currentFile.IsOpen = true;
                    if (!currentFile.IsSave) currentFile.IsSave = true;
                    currentFile.FilePath = path;
                    currentFile.FileName = StringHelper.GetFileName(path);
                    currentFile.FileExt = StringHelper.GetFileExt(currentFile.FileName);

                    // 把标题改为另存为的文件的名称
                    Title = currentFile.FileName + "-CCT通用配置工具";
                    Status = currentFile.FilePath;

                    //CurrentViewModel = new MainXmlViewModel();
                }
            }
            else
            {
                var path = string.Empty;
                // 获取另存为路径
                if (!string.IsNullOrWhiteSpace(currentFile.FileExt))
                {
                    path = FileDialogHelper.SaveAsFile(currentFile.FileExt);
                }
                else
                {
                    path = FileDialogHelper.SaveAsFile("txt");
                }
                if (!string.IsNullOrWhiteSpace(path))
                {
                    // 调用另存为指定txt地址的文件
                    RichTextBoxHelper.SaveFile(path, richTextBox);

                    // 初始化加载文件属性
                    if (!currentFile.IsOpen) currentFile.IsOpen = true;
                    if (!currentFile.IsSave) currentFile.IsSave = true;

                    currentFile.FilePath = path;
                    currentFile.FileName = StringHelper.GetFileName(path);
                    currentFile.FileExt = StringHelper.GetFileExt(currentFile.FileName);

                    // 把标题改为另存为的文件的名称
                    Title = currentFile.FileName + "-CCT通用配置工具";
                    Status = currentFile.FilePath;
                }
            }
        }
        #endregion

        #region 解析

        /// <summary>
        /// 解析
        /// </summary>
        private void ParseCommandExecute(Window main)
        {
            if (string.IsNullOrWhiteSpace(currentFile?.FilePath))
            {
                MessageBox.Show("未检测到文件路径!", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                switch(currentFile.FileExt)
                {
                    case "xml":
                        //打开Xml解析窗口
                        new XmlParseWindow()
                        {
                            Owner=main,
                            DataContext = new XmlParseWindowViewModel(currentFile)
                        }.ShowDialog();
                        break;
                    case "ini":
                        //打开InI解析窗口
                        new InIParseWindow()
                        {
                            Owner = main,
                            DataContext = new InIParseWindowViewModel(currentFile)
                        }.ShowDialog();
                        break;
                    case "json":
                        //打开Json解析窗口
                        new JsonParseWindow()
                        {
                            Owner = main,
                            DataContext = new JsonParseWindowViewModel(currentFile)
                        }.ShowDialog();
                        break;
                    case "properties":
                        //打开Properties解析窗口
                        new PropertiesParseWindow()
                        {
                            Owner = main,
                            DataContext = new PropertiesParseWindowViewModel(currentFile)
                        }.ShowDialog();
                        break;
                    default:break;
                }
            } 
        }

        #endregion

        #region 导出

        /// <summary>
        /// 导出为文本
        /// </summary>
        private void ExportTCommandExecute(RichTextBox richTextBox)
        {
            if (currentFile == null) return;

            try
            {
                // 获取另存为路径
                var path = FileDialogHelper.SaveAsFile("txt");

                if (!string.IsNullOrWhiteSpace(path))
                {
                    // 调用另存为指定txt地址的文件
                    RichTextBoxHelper.SaveFile(path, richTextBox);

                    // 初始化加载文件属性
                    if (!currentFile.IsOpen) currentFile.IsOpen = true;
                    if (!currentFile.IsSave) currentFile.IsSave = true;

                    currentFile.FilePath = path;
                    currentFile.FileName = StringHelper.GetFileName(path);
                    currentFile.FileExt = StringHelper.GetFileExt(currentFile.FileName);

                    // 把标题改为另存为的文件的名称
                    Title = currentFile.FileName + "-CCT通用配置工具";

                    Status = currentFile.FilePath;

                    MessageBox.Show("导出成功!", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {
                MessageBox.Show("导出失败!", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 导出为word
        /// </summary>
        private void ExportWCommandExecute(RichTextBox richTextBox)
        {
            if (currentFile == null) return;
            var path = FileDialogHelper.SaveAsFile("doc");
            try
            {
                WordGenerateHelper.SaveAsWord(path,richTextBox);
                MessageBox.Show("WORD文件保存成功", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception Err)
            {
                MessageBox.Show("WORD文件保存操作失败！" + Err.Message, "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 导出为图片
        /// </summary>
        private void ExportPCommandExecute(RichTextBox richTextBox)
        {
            if (currentFile == null) return;
            try
            {
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)richTextBox.ActualWidth, (int)richTextBox.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(richTextBox);

                var path = FileDialogHelper.SaveAsFile("png");

                using (Stream fs = File.Create(path))
                {
                    ImageGenerateHelper.GenerateImage(rtb, ImageFormat.PNG, fs);
                }
                MessageBox.Show("导出成功!", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("导出失败!", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #region 编辑

        /// <summary>
        /// 撤销
        /// </summary>
        private void UndoCommandExecute(RichTextBox richTextBox)
        {
            if (richTextBox?.CanUndo == true)
            {
                richTextBox.Undo();
            }
        }
        /// <summary>
        /// 重做
        /// </summary>
        private void RedoCommandExecute(RichTextBox richTextBox)
        {
            if (richTextBox?.CanRedo == true)
            {
                richTextBox.Redo();
            }
        }
        /// <summary>
        /// 剪切
        /// </summary>
        private void CutCommandExecute(RichTextBox richTextBox)
        {
            if (richTextBox == null) return;

            if (!richTextBox.Selection.IsEmpty)
            {
                richTextBox.Cut();
            }
        }
        /// <summary>
        /// 复制
        /// </summary>
        private void CopyCommandExecute(RichTextBox richTextBox)
        {
            if (richTextBox == null) return;

            if (!richTextBox.Selection.IsEmpty)
            {
                richTextBox.Copy();
            }
        } 

        /// <summary>
        /// 粘贴
        /// </summary>
        private void PasteCommandExecute(RichTextBox richTextBox)
        {
            if (richTextBox == null) return;

            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                richTextBox.Paste();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void DelCommandExecute(RichTextBox richTextBox)
        {
            if (richTextBox == null) return;

            if (!richTextBox.Selection.IsEmpty)
            {
                richTextBox.Selection.Text = string.Empty;
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        private void SelAllCommandExecute(RichTextBox richTextBox)
        {
            if (richTextBox == null) return;

            richTextBox.SelectAll();
        }

        #endregion

        #region 最近

        /// <summary>
        /// 打开上次
        /// </summary>
        private void RecentOpenCommandExecute(RichTextBox richTextBox)
        {
            if(!string.IsNullOrWhiteSpace(SavedLastOpenFile.X2))
            {
               OpenFilePath(SavedLastOpenFile.X2, richTextBox);
            }
        }

        /// <summary>
        /// 打开最近
        /// </summary>
        private void RecentFileCommandExecute(MenuItem menuItem)
        {
            var path = menuItem.Tag.ToString();
            Window win = VisualTreeHelpers.GetParentObject<Window>(menuItem,"mainWindow");
            if(win!=null)
            {
                RichTextBox richTextBox = VisualTreeHelpers.GetChildObject<RichTextBox>(win, "richTextBox");
                if(richTextBox!=null)
                {
                    OpenFilePath(path, richTextBox);
                }
            }
        }

        #endregion

        #region 退出

        /// <summary>
        /// 保存退出
        /// </summary>
        /// <param name="richTextBox"></param>
        private void SaveExitCommandExecute(RichTextBox richTextBox)
        {
            if (richTextBox == null) return;
            SaveCommandExecute(richTextBox);
            Window win = VisualTreeHelpers.GetParentObject<Window>(richTextBox, "mainWindow");
            if (win != null)
            {
                //保存系统配置
                if (ConfigHelper.SaveSysConfig(SysConfig))
                {
                    UpdateQuiteDate();
                    win.Close();
                }
            }
        }

        /// <summary>
        /// 不保存退出
        /// </summary>
        /// <param name="win"></param>
        private void ExitCommandExecute(Window win)
        {
            if (win != null)
            {
                //保存系统配置
                if (ConfigHelper.SaveSysConfig(SysConfig))
                {
                    UpdateQuiteDate();
                    win.Close();
                }
            }
        }

        #endregion

        #region 帐户

        /// <summary>
        /// 查看帐户
        /// </summary>
        private void LookUserCommandExecute(Window main)
        {      
            Person win = new Person()
            {
                DataContext = new PersonViewModel(CurrentUser)
            };
            win.Owner = main;
            win.ShowDialog();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        private void UpdatePwdCommandExecute(Window main)
        {
            UpdatePwd win = new UpdatePwd()
            {
                DataContext = new UpdatePwdViewModel(CurrentUser)
            };
            win.Owner = main;
            win.ShowDialog();
        }

        /// <summary>
        /// 修改电话
        /// </summary>
        private void UpdateTelCommandExecute(Window main)
        {
            UpdateTel win = new UpdateTel()
            {
                DataContext = new UpdateTelViewModel(CurrentUser)
            };
            win.Owner = main;
            win.ShowDialog();
        }

        #endregion

        #region 文本

        /// <summary>
        /// 编辑框文本改变处理
        /// </summary>
        private void RichTextBoxTextChangedCommandExecute(RichTextBox richTextBox)
        {
            if (currentFile == null)
            {
                //初始化当前文件类
                CurrentFile = new LoadedFile();
            }

            // 把已保存状态改成未保存状态
            if(currentFile.IsSave) currentFile.IsSave = false;

            if (currentFile.IsOpen)
            {
                Title = "*" + currentFile.FileName + "-CCT通用配置工具"; 
            }
            else
            {
                Title = "*未命名-CCT通用配置工具";
            }
        }

        /// <summary>
        /// 编辑框选中文本改变处理
        /// </summary>
        private void RichTextBoxSelectionChangedCommandExecute(RichTextBox richTextBox)
        {
            if (currentFile == null) return;

            if(richTextBox.Selection.Text.Equals(string.Empty))
            {
                if(currentFile.IsEnabled) currentFile.IsEnabled = false;
            }
            else
            {
                if (!currentFile.IsEnabled) currentFile.IsEnabled = true;
            }
        } 

        #endregion

        #region 打开文件

        /// <summary>
        /// 打开指定类型的文件
        /// </summary>
        /// <param name="format"></param>
        /// <param name="richTextBox"></param>
        public void OpenFile(string format,RichTextBox richTextBox)
        {
            if (richTextBox == null) return;

            //初始化当前文件类
            CurrentFile = new LoadedFile();

            if (!currentFile.IsSave)
            {
                MessageBoxResult result = MessageBox.Show("是否将更改保存？", "CCT温馨提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    SaveCommandExecute(richTextBox);
                }
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            if (!currentFile.IsOpen) currentFile.IsOpen = true;

            // 获取文件路径
            var filepath = FileDialogHelper.OpenFile(format);

            if (!string.IsNullOrWhiteSpace(filepath))
            {
                // 加载指定地址的文件
                RichTextBoxHelper.LoadFile(filepath, richTextBox);

                // 初始化加载文件属性
                if (!currentFile.IsSave) currentFile.IsSave = true;
                currentFile.FilePath = filepath;
                currentFile.FileName = StringHelper.GetFileName(filepath);
                currentFile.FileExt = StringHelper.GetFileExt(currentFile.FileName).ToLower();

                // 把标题改为打开的文件的名称
                Title = currentFile.FileName + "-CCT通用配置工具";
                // 改变状态栏
                Status = currentFile.FilePath;

                //不包含才更新
                var flg1 = !SavedLastOpenFile.X2.Equals(currentFile);
                var flg2 = !List.Select(x => x.X2).Contains(currentFile.FilePath);
                if (flg1)
                {
                    SavedLastOpenFile.X1 = currentFile.FileName;
                    SavedLastOpenFile.X2 = currentFile.FilePath;
                    SysConfig.SavedLastOpenFile = SavedLastOpenFile;
                }
                if (flg2)
                {
                    List[0] = List[1];
                    List[1] = List[2];
                    List[2] = new RecentFile()
                    {
                        X1 = currentFile.FileName,
                        X2 = currentFile.FilePath
                    };
                    SysConfig.SavedRecentFile.List = List;
                }
                if (flg1 || flg2)
                {
                    ConfigHelper.SaveSysConfig(SysConfig);
                }

                //CurrentViewModel = new MainXmlViewModel();
            }
        }

        /// <summary>
        /// 打开指定地址的文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="richTextBox"></param>
        public void OpenFilePath(string filepath, RichTextBox richTextBox)
        {
            if (richTextBox == null) return;

            //初始化当前文件类
            CurrentFile = new LoadedFile();

            if (!currentFile.IsSave)
            {
                MessageBoxResult result = MessageBox.Show("是否将更改保存？", "CCT温馨提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    SaveCommandExecute(richTextBox);
                }
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            if (!currentFile.IsOpen) currentFile.IsOpen = true;

            if (!string.IsNullOrWhiteSpace(filepath))
            {
                // 加载指定地址的文件
                RichTextBoxHelper.LoadFile(filepath, richTextBox);

                // 初始化加载文件属性
                if (!currentFile.IsSave) currentFile.IsSave = true;
                currentFile.FilePath = filepath;
                currentFile.FileName = StringHelper.GetFileName(filepath);
                currentFile.FileExt = StringHelper.GetFileExt(currentFile.FileName).ToLower();

                // 把标题改为打开的文件的名称
                Title = currentFile.FileName + "-CCT通用配置工具";
                // 改变状态栏
                Status = currentFile.FilePath;

                SavedLastOpenFile.X1 = currentFile.FileName;
                SavedLastOpenFile.X2 = currentFile.FilePath;
                SysConfig.SavedLastOpenFile = SavedLastOpenFile;

                List[0] = List[1];
                List[1] = List[2];
                List[2] = new RecentFile()
                {
                    X1 = currentFile.FileName,
                    X2 = currentFile.FilePath
                };
                SysConfig.SavedRecentFile.List = List;

                ConfigHelper.SaveSysConfig(SysConfig);

                //CurrentViewModel = new MainXmlViewModel();
            }
        }

        #endregion

        #region 更新退出时间

        /// <summary>
        /// 更新登录信息到数据库
        /// </summary>
        private void UpdateQuiteDate()
        {
            CurrentUser.UserQuiteDate = DateTime.Now;
            UserService.UpdateUserQuiteDate(CurrentUser);
        }

        #endregion
    }
}
