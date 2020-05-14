using CCT.Model.DataType;
using CCT.Model.Helper;
using CCT.Resource.Helpers;
using CCT.View;
using Prism.Commands;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

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

        #region 命令绑定
        public ICommand OpenXmlCommand { get; private set; }//打开XML文件
        public ICommand OpenJsonCommand { get; private set; }//打开Json文件
        public ICommand OpenInICommand { get; private set; }//打开InI文件
        public ICommand OpenPropertiesCommand { get; private set; }//打开Properties文件

        public ICommand SaveCommand { get; private set; }//保存
        public ICommand SaveAsCommand { get; private set; }//另存为

        public ICommand LookUserCommand { get; private set; }//查看帐户
        public ICommand UpdatePwdCommand { get; private set; }//修改密码
        public ICommand UpdateTelCommand { get; private set; }//修改电话

        public ICommand RichTextBoxTextChangedCommand { get; private set; }//文本变化事件
        public ICommand RichTextBoxSelectionChangedCommand { get; private set; }//选中文本变化事件
        #endregion

        #region 构造方法

        public MainWindowViewModel(User user)
        {
            //CurrentUser = user;
            CurrentUser = new User();
            OpenXmlCommand = new DelegateCommand<RichTextBox>(OpenXmlCommandExecute);
            OpenJsonCommand = new DelegateCommand(OpenJsonCommandExecute);
            OpenInICommand = new DelegateCommand(OpenInICommandExecute);
            OpenPropertiesCommand = new DelegateCommand(OpenPropertiesCommandExecute);

            SaveCommand= new DelegateCommand<RichTextBox>(SaveCommandExecute);
            SaveAsCommand = new DelegateCommand<RichTextBox>(SaveAsCommandExecute);

            LookUserCommand = new DelegateCommand<Window>(LookUserCommandExecute);
            UpdatePwdCommand = new DelegateCommand<Window>(UpdatePwdCommandExecute);
            UpdateTelCommand = new DelegateCommand<Window>(UpdateTelCommandExecute);

            RichTextBoxTextChangedCommand = new DelegateCommand<RichTextBox>(RichTextBoxTextChangedCommandExecute);
            RichTextBoxSelectionChangedCommand = new DelegateCommand<RichTextBox>(RichTextBoxSelectionChangedCommandExecute);
        }

        public MainWindowViewModel()
        {
            CurrentUser = new User();
            OpenXmlCommand = new DelegateCommand<RichTextBox>(OpenXmlCommandExecute);
            OpenJsonCommand = new DelegateCommand(OpenJsonCommandExecute);
            OpenInICommand = new DelegateCommand(OpenInICommandExecute);
            OpenPropertiesCommand = new DelegateCommand(OpenPropertiesCommandExecute);

            SaveCommand = new DelegateCommand<RichTextBox>(SaveCommandExecute);
            SaveAsCommand = new DelegateCommand<RichTextBox>(SaveAsCommandExecute);

            LookUserCommand = new DelegateCommand<Window>(LookUserCommandExecute);
            UpdatePwdCommand = new DelegateCommand<Window>(UpdatePwdCommandExecute);
            UpdateTelCommand = new DelegateCommand<Window>(UpdateTelCommandExecute);

            RichTextBoxTextChangedCommand = new DelegateCommand<RichTextBox>(RichTextBoxTextChangedCommandExecute);
            RichTextBoxSelectionChangedCommand = new DelegateCommand<RichTextBox>(RichTextBoxSelectionChangedCommandExecute);
        }

        #endregion

        #region Xml文件
        /// <summary>
        /// 打开XML文件
        /// </summary>
        private void OpenXmlCommandExecute(RichTextBox richTextBox)
        {
            //初始化当前文件类
            CurrentFile = new LoadedFile();

            if (!currentFile.IsSave)
            {
                MessageBoxResult result = MessageBox.Show("是否将更改保存？", "CCT温馨提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    //Savedata();
                }
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            if(!currentFile.IsOpen) currentFile.IsOpen = true;

            // 获取打开xml文件路径
            var filepath = FileDialogHelper.OpenFile("xml");

            if (!string.IsNullOrWhiteSpace(filepath))
            {
                // 调用加载指定xml地址的文件
                RichTextBoxHelper.LoadFile(filepath, richTextBox);

                // 初始化加载文件属性
                if(!currentFile.IsSave) currentFile.IsSave = true;
                currentFile.FilePath = filepath;
                currentFile.FileName = StringHelper.GetFileName(filepath);
                currentFile.FileExt = StringHelper.GetFileExt(currentFile.FileName);

                // 把标题改为打开的文件的名称
                Title = currentFile.FileName + "-CCT通用配置工具";
                Status = currentFile.FilePath;

                //CurrentViewModel = new MainXmlViewModel();
            }
        }
        #endregion

        #region Json文件
        /// <summary>
        /// 打开JSON文件
        /// </summary>
        private void OpenJsonCommandExecute()
        {
            var filepath = FileDialogHelper.OpenFile("json");

            CurrentViewModel = new MainXmlViewModel();
        }
        #endregion

        #region InI文件
        /// <summary>
        /// 打开INI文件
        /// </summary>
        private void OpenInICommandExecute()
        {
            var filepath = FileDialogHelper.OpenFile("ini");
            CurrentViewModel = new MainXmlViewModel();
        }
        #endregion

        #region Properties文件
        /// <summary>
        /// 打开Properties文件
        /// </summary>
        private void OpenPropertiesCommandExecute()
        {

            var filepath = FileDialogHelper.OpenFile("properties");
            CurrentViewModel = new MainXmlViewModel();
        }
        #endregion

        #region 文件保存
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

                // 把标题改为另存为的文件的名称
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

        #region 文件另存为
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
                }
            }
        }
        #endregion

        #region 帐户相关

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

        #region 编辑框相关处理

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

        #region 私有方法



        #endregion
    }
}
