using CCT.Model.DataType;
using CCT.Resource.Enums;
using CCT.Resource.Helpers;
using CCT.Resource.Helpers.FileHelper;
using Microsoft.VisualBasic;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace CCT.ViewModel
{
    public class JsonParseWindowViewModel : ViewModelBase
    {
        #region 私有域

        private ViewModelStatus _status = ViewModelStatus.None;//异步状态

        private LoadedFile currentFile;//当前文件

        private JsonHelper jsonHelper;//属性文件助手

        private Node currentNode;//当前节点

        private bool isLoading = false;//是否显示加载

        private bool isCurrentNodeNull = true;//当前节点是否为空

        #endregion

        #region 属性

        /// <summary>
        /// 数据源根节点
        /// </summary>
        public Node RootNode { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public ObservableCollection<Node> RootNodeList { get; set; }

        /// <summary>
        /// 当前选中节点
        /// </summary>
        public Node CurrentNode
        {
            get { return currentNode; }
            set
            {
                SetProperty(ref currentNode, value);
                if(currentNode==null)
                {
                    IsCurrentNodeNull = true;
                }
                else
                {
                    IsCurrentNodeNull = false;
                }
            }
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
        /// 是否显示加载
        /// </summary>
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        /// <summary>
        /// ViewModel状态
        /// </summary>
        public ViewModelStatus _Status
        {
            get { return _status; }
            protected set
            {
                if (_status != value)
                {
                    _status = value;
                    RaisePropertyChanged(@"_Status");
                }
            }
        }

        /// <summary>
        /// 当前节点是否为空
        /// </summary>
        public bool IsCurrentNodeNull
        {
            get { return isCurrentNodeNull; }
            set { SetProperty(ref isCurrentNodeNull, value); }
        }

        #endregion

        #region 命令

        public ICommand SaveCommand { get; private set; }//保存
        public ICommand ExportExcelCommand { get; private set; }//导出Excel
        public ICommand ExportXmlCommand { get; private set; }//导出Xml

        public ICommand InsertCommand { get; private set; }//插入
        public ICommand RemoveCommand { get; private set; }//移除
        public ICommand ClearCommand { get; private set; }//清除
        public ICommand ParseCommand { get; private set; }//恢复

        public ICommand GotFocusCommand { get; private set; }//切换至Xml

        public ICommand RichTextBoxSelectionChangedCommand { get; private set; }//选中文本变化事件

        #endregion

        #region 构造方法

        public JsonParseWindowViewModel()
        {
            Title = "解析Json-CCT通用配置工具";
        }

        public JsonParseWindowViewModel(LoadedFile file)
        {
            Title = "解析Json-CCT通用配置工具";

            CurrentFile = file;

            jsonHelper = new JsonHelper(currentFile.FilePath);

            ExportExcelCommand = new DelegateCommand(ExportExcelCommandExecute);
            ExportXmlCommand = new DelegateCommand(ExportXmlCommandExecute);
            SaveCommand = new DelegateCommand(SaveCommandExecute);

            InsertCommand = new DelegateCommand(InsertCommandExecute);
            RemoveCommand = new DelegateCommand(RemoveCommandExecute);
            ClearCommand = new DelegateCommand<RichTextBox>(ClearCommandExecute);
            ParseCommand = new DelegateCommand(ParseCommandExecute);

            GotFocusCommand = new DelegateCommand<RichTextBox>(GotFocusCommandExecute);
            RichTextBoxSelectionChangedCommand = new DelegateCommand<RichTextBox>(RichTextBoxSelectionChangedCommandExecute);

            RootNode = new Node() { CanRemove = false };//初始化根节点
            ParseFile();
            RootNodeList = new ObservableCollection<Node>();
            RootNodeList.Add(RootNode);
        }

        #endregion

        #region 解析

        /// <summary>
        /// 解析文件
        /// </summary>
        private void ParseFile()
        {
            if (!jsonHelper.Load(RootNode))
            {
                MessageBox.Show("解析异常，请检查Json格式是否正确！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 重新恢复
        /// </summary>
        private void ParseCommandExecute()
        {
            RootNodeList.Add(RootNode);
        }

        #endregion

        #region 导出

        /// <summary>
        /// 导出为Excel
        /// </summary>
        private void ExportExcelCommandExecute()
        {
            _Status = ViewModelStatus.Initializing;

            // 获取路径
            var path = FileDialogHelper.SaveAsFile("xlsx");

            IsLoading = true;

            if (string.IsNullOrWhiteSpace(path))
            {
                IsLoading = false;
                return;
            }

            if (jsonHelper.ExportExcel(path, RootNode))
            {
                _Status = ViewModelStatus.Loaded;
                MessageBox.Show("已导出！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                IsLoading = false;
                return;
            }
            _Status = ViewModelStatus.Loaded;
            IsLoading = false;
            MessageBox.Show("导出异常！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 导出为Xml
        /// </summary>
        private void ExportXmlCommandExecute()
        {
            _Status = ViewModelStatus.Initializing;

            // 获取路径
            var path = FileDialogHelper.SaveAsFile("xml");

            IsLoading = true;

            if (string.IsNullOrWhiteSpace(path))
            {
                IsLoading = false;
                return;
            }

            if (jsonHelper.ExportXml(path))
            {
                _Status = ViewModelStatus.Loaded;
                MessageBox.Show("已导出！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                IsLoading = false;
                return;
            }
            _Status = ViewModelStatus.Loaded;
            IsLoading = false;
            MessageBox.Show("导出异常,请检查json格式是否正确！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        private void SaveCommandExecute()
        {
            if (jsonHelper.Save(RootNode))
            {
                MessageBox.Show("已保存！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            MessageBox.Show("保存异常！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        #region 插入

        /// <summary>
        /// 插入
        /// </summary>
        private void InsertCommandExecute()
        {
            if (CurrentNode != null)
            {
                //获取输入
                string nodeName = GetUserInput("注意：输入不能为空且不能与已有孩子节点名称重复\r\nArray结构为数值，Object结构为键值对！", "请输入节点名称", "");
                if (!string.IsNullOrWhiteSpace(nodeName))
                {
                    if (CurrentNode.Children.Count(x => x.DisplayName.Equals(nodeName)) == 0)
                    {
                        Node node = new Node() { Parent=CurrentNode };
                        if (CurrentNode.DisplayName.Equals(JsonType.Array.ToString()))
                        {
                            if (nodeName.Contains(":"))
                            {
                                MessageBox.Show("名称格式不正确，创建失败！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }
                        }
                        else if (CurrentNode.DisplayName.Equals(JsonType.Object.ToString()))
                        {
                            if(!nodeName.Contains(":"))
                            {
                                MessageBox.Show("名称格式不正确，创建失败！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }
                        }
                        node.DisplayName = nodeName;
                        CurrentNode.Children.Add(node);
                        NotifyUI();
                    }
                    else
                    {
                        MessageBox.Show("名称重复，创建失败！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除
        /// </summary>
        private void RemoveCommandExecute()
        {
            if(CurrentNode!=null)
            {
                if(CurrentNode.Parent!=null)
                {
                    CurrentNode.Parent.Children.Remove(CurrentNode);
                    NotifyUI();
                }
            } 
        }

        #endregion

        #region 清除

        /// <summary>
        /// 清除
        /// </summary>
        private void ClearCommandExecute(RichTextBox richTextBox)
        {
            MessageBoxResult result = MessageBox.Show("确定要一键清除吗？", "信息提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                RootNodeList.Clear();
                TextRange documentTextRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                documentTextRange.Text=string.Empty;
            }
            NotifyUI();
        }

        #endregion

        #region 模式

        /// <summary>
        /// 切换至Xml
        /// </summary>
        /// <param name="richTextBox"></param>
        private void GotFocusCommandExecute(RichTextBox richTextBox)
        {
            if (RootNodeList.Count != 0)//左侧树存在数据时可以切换
            {
                TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);//获取文本对象
                if (string.IsNullOrWhiteSpace(textRange.Text))
                {
                    if (!jsonHelper.DisplayXml(richTextBox))
                    {
                        richTextBox.Document.Blocks.Clear();
                        MessageBox.Show("转换xml异常,请检查json格式是否正确！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        #endregion

        #region 文本

        /// <summary>
        /// 编辑框选中文本改变处理
        /// </summary>
        private void RichTextBoxSelectionChangedCommandExecute(RichTextBox richTextBox)
        {
            if (currentFile == null) return;

            if (richTextBox.Selection.Text.Equals(string.Empty))
            {
               if (currentFile.IsEnabled) currentFile.IsEnabled = false;
            }
            else
            {
                if (!currentFile.IsEnabled) currentFile.IsEnabled = true;
            }
        }

        #endregion

        #region 通知

        /// <summary>
        /// 通知Converter变化
        /// </summary>
        public void NotifyUI()
        {
            RaisePropertyChanged(nameof(RootNodeList));
            RaisePropertyChanged(nameof(CurrentNode));
        }

        #endregion

        #region 输入

        /// <summary>
        /// 获取用户输入
        /// </summary>
        /// <param name="tip"></param>
        /// <param name="title"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        private string GetUserInput(string tip, string title, string def)
        {
            return Interaction.InputBox(tip, title, def, -1, -1);
        }

        #endregion
    }
}
