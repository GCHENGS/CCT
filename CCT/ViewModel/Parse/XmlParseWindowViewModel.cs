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
    public class XmlParseWindowViewModel : ViewModelBase
    {
        #region 私有域

        private LoadedFile currentFile;//当前文件

        private XmlHelper xmlHelper;//属性文件助手

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
        /// 属性数据源
        /// </summary>
        public ObservableCollection<PropertyItem> PropertyItemList { get; set; }

        /// <summary>
        /// 当前选中节点
        /// </summary>
        public Node CurrentNode
        {
            get { return currentNode; }
            set
            {
                SetProperty(ref currentNode, value);
                if (currentNode == null)
                {
                    IsCurrentNodeNull = true;
                }
                else
                {
                    IsCurrentNodeNull = false;
                    PropertyItemList.Clear();
                    PropertyItemList.AddRange(CurrentNode.Attributies);
                    NotifyUI();
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
        public ICommand ExportNodeExcelCommand { get; private set; }//导出节点Excel
        public ICommand ExportAttributeExcelCommand { get; private set; }//导出属性Excel
        public ICommand ExportJsonCommand { get; private set; }//导出json

        public ICommand InsertCommand { get; private set; }//插入
        public ICommand RemoveCommand { get; private set; }//移除
        public ICommand ClearCommand { get; private set; }//清除
        public ICommand ParseCommand { get; private set; }//恢复

        public ICommand GotFocusCommand { get; private set; }//切换至Json
        public ICommand RichTextBoxSelectionChangedCommand { get; private set; }//选中文本变化事件

        public ICommand SelectAllKeyCommand { get; private set; }//全选
        public ICommand DelBatchKeyCommand { get; private set; }//批量删除
        public ICommand AddKeyCommand { get; private set; }//添加
        public ICommand ClearKeyCommand { get; private set; }//清除

        #endregion

        #region 构造方法

        public XmlParseWindowViewModel()
        {
            Title = "解析Xml-CCT通用配置工具";
        }

        public XmlParseWindowViewModel(LoadedFile file)
        {
            Title = "解析Xml-CCT通用配置工具";

            CurrentFile = file;

            xmlHelper = new XmlHelper(currentFile.FilePath);

            ExportNodeExcelCommand = new DelegateCommand(ExportNodeExcelCommandExecute);
            ExportAttributeExcelCommand = new DelegateCommand(ExportAttributeExcelCommandExecute);
            ExportJsonCommand = new DelegateCommand(ExportJsonCommandExecute);
            SaveCommand = new DelegateCommand(SaveCommandExecute);

            InsertCommand = new DelegateCommand(InsertCommandExecute);
            RemoveCommand = new DelegateCommand(RemoveCommandExecute);
            ClearCommand = new DelegateCommand<RichTextBox>(ClearCommandExecute);
            ParseCommand = new DelegateCommand(ParseCommandExecute);

            GotFocusCommand = new DelegateCommand<RichTextBox>(GotFocusCommandExecute);
            RichTextBoxSelectionChangedCommand = new DelegateCommand<RichTextBox>(RichTextBoxSelectionChangedCommandExecute);

            SelectAllKeyCommand = new DelegateCommand(SelectAllKeyCommandExecute);
            DelBatchKeyCommand = new DelegateCommand(DelBatchKeyCommandExecute);
            AddKeyCommand = new DelegateCommand(AddKeyCommandExecute);
            ClearKeyCommand = new DelegateCommand(ClearKeyCommandExecute);

            RootNode = new Node() { CanRemove = false,NodeType=NodeType.Element.ToString() };//初始化根节点
            ParseFile();

            RootNodeList = new ObservableCollection<Node>();
            PropertyItemList = new ObservableCollection<PropertyItem>();

            RootNodeList.Add(RootNode);
        }

        #endregion

        #region 解析

        /// <summary>
        /// 解析文件
        /// </summary>
        private void ParseFile()
        {
            if (!xmlHelper.Load(RootNode,this))
            {
                MessageBox.Show("解析异常，请检查Xml格式是否正确！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 重新恢复
        /// </summary>
        private void ParseCommandExecute()
        {
            RootNodeList.Add(RootNode);
            NotifyUI();
        }

        #endregion

        #region 导出

        /// <summary>
        /// 导出Node为Excel
        /// </summary>
        private void ExportNodeExcelCommandExecute()
        {
            // 获取路径
            var path = FileDialogHelper.SaveAsFile("xlsx");

            IsLoading = true;

            if (string.IsNullOrWhiteSpace(path))
            {
                IsLoading = false;
                return;
            }

            if (xmlHelper.ExportNodeExcel(path, RootNode))
            {
                MessageBox.Show("已导出！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                IsLoading = false;
                return;
            }

            IsLoading = false;
            MessageBox.Show("导出异常！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 导出Attribute为Excel
        /// </summary>
        private void ExportAttributeExcelCommandExecute()
        {
            // 获取路径
            var path = FileDialogHelper.SaveAsFile("xlsx");

            IsLoading = true;

            if (string.IsNullOrWhiteSpace(path))
            {
                IsLoading = false;
                return;
            }

            if (xmlHelper.ExportAttributeExcel(path, RootNode))
            {
                MessageBox.Show("已导出！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                IsLoading = false;
                return;
            }

            IsLoading = false;
            MessageBox.Show("导出异常！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 导出为Json
        /// </summary>
        private void ExportJsonCommandExecute()
        {
            // 获取路径
            var path = FileDialogHelper.SaveAsFile("json");

            IsLoading = true;

            if (string.IsNullOrWhiteSpace(path))
            {
                IsLoading = false;
                return;
            }

            if (xmlHelper.ExportXml(path))
            {
                MessageBox.Show("已导出！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                IsLoading = false;
                return;
            }

            IsLoading = false;
            MessageBox.Show("导出异常,请检查Xml格式是否正确！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        private void SaveCommandExecute()
        {
            if (xmlHelper.Save(RootNode))
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
                        Node node = new Node() { Parent = CurrentNode };
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
                            if (!nodeName.Contains(":"))
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
            if (CurrentNode != null)
            {
                if (CurrentNode.Parent != null)
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
                documentTextRange.Text = string.Empty;
            }
            NotifyUI();
        }

        #endregion

        #region 属性操作

        /// <summary>
        /// 全选
        /// </summary>
        private void SelectAllKeyCommandExecute()
        {
            if (PropertyItemList.Count == 0)
            {
                return;
            }
            var selectedlist = PropertyItemList.Where(x => x.IsChecked);
            if (selectedlist.Count() == PropertyItemList.Count)
            {
                //执行反选
                PropertyItemList.ToList().ForEach(x => x.IsChecked = false);
            }
            else
            {
                //执行全选
                PropertyItemList.Where(x => x.IsChecked == false).ToList().ForEach(x => x.IsChecked = true);
            }
            NotifyUI();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        private void DelBatchKeyCommandExecute()
        {
            if (PropertyItemList.Count == 0)
            {
                return;
            }
            if (PropertyItemList.Count(x => x.IsChecked) == 0)
            {
                MessageBox.Show("请至少选择一项！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            for (int i = 0; i < PropertyItemList.Count; i++)
            {
                var item = PropertyItemList[i];
                if (item.IsChecked)
                {
                    if (CurrentNode != null)
                    {
                        PropertyItemList.Remove(item);
                        CurrentNode.Attributies.Remove(item);
                        i = -1;
                    }
                }
            }
            NotifyUI();
        }

        /// <summary>
        /// 添加
        /// </summary>
        private void AddKeyCommandExecute()
        {
            //获取输入
            string keyName = GetUserInput("注意：输入不能为空且不能重复！", "请输入Key名称", "");
            if (!string.IsNullOrWhiteSpace(keyName))
            {
                if (PropertyItemList.Count(x => x.KeyName.Equals(keyName)) == 0)
                {
                    if (CurrentNode != null)
                    {
                        PropertyItem newItem = new PropertyItem(CurrentNode,this)
                        {
                            KeyName = keyName
                        };
                        PropertyItemList.Add(newItem);
                        CurrentNode.Attributies.Add(newItem);
                        NotifyUI();
                    }
                }
                else
                {
                    MessageBox.Show("Key重复，创建失败！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// 清除
        /// </summary>
        private void ClearKeyCommandExecute()
        {
            if(PropertyItemList.Count==0)
            {
                return;
            }
            MessageBoxResult result = MessageBox.Show("确定要一键清除吗？", "信息提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                if (CurrentNode != null)
                {
                    PropertyItemList.Clear();
                    CurrentNode.Attributies.Clear();
                }
            }
            NotifyUI();
        }

        #endregion

        #region 模式

        /// <summary>
        /// 切换至Json
        /// </summary>
        /// <param name="richTextBox"></param>
        private void GotFocusCommandExecute(RichTextBox richTextBox)
        {
            if (RootNodeList.Count != 0)//左侧树存在数据时可以切换
            {
                TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);//获取文本对象
                if (string.IsNullOrWhiteSpace(textRange.Text))
                {
                    if (!xmlHelper.DisplayJson(richTextBox))
                    {
                        richTextBox.Document.Blocks.Clear();
                        MessageBox.Show("转换xml异常,请检查Xml格式是否正确！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
            RaisePropertyChanged(nameof(PropertyItemList));
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
