using CCT.Model.DataType;
using CCT.Resource.Helpers;
using CCT.Resource.Helpers.FileHelper;
using Microsoft.VisualBasic;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CCT.ViewModel
{
    public class PropertiesParseWindowViewModel : ViewModelBase
    {
        #region 私有域

        private string searchText;//搜索字符串

        private LoadedFile currentFile;//当前文件

        private PropertiesHelper propertiesHelper;//属性文件助手

        private PropertyItem currentPropertyItem;//当前配置

        private bool isLoading = false;//是否显示加载

        #endregion

        #region 属性

        /// <summary>
        /// 搜索字符串
        /// </summary>
        public string SearchText
        {
            get { return searchText; }
            set
            {
                SetProperty(ref searchText, value);

                FilterPropertyItemListView?.Refresh();

                NotifyUI();
            }
        }

        /// <summary>
        /// 当前配置
        /// </summary>
        public PropertyItem CurrentPropertyItem
        {
            get { return currentPropertyItem; }
            set
            {
                if (value == null) return;
                SetProperty(ref currentPropertyItem, value);
            }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public ObservableCollection<PropertyItem> PropertyItemList { get; set; }
    
        /// <summary>
        /// 映射View
        /// </summary>
        public ICollectionView FilterPropertyItemListView
        {
            get
            {
                return CollectionViewSource.GetDefaultView(PropertyItemList);
            }
        }

        /// <summary>
        /// 是否显示加载
        /// </summary>
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        #endregion

        #region 命令

        public ICommand ExportExcelCommand { get; private set; }//导出
        public ICommand SaveCommand { get; private set; }//保存

        public ICommand SelectAllKeyCommand { get; private set; }//全选
        public ICommand DelKeyCommand { get; private set; }//删除
        public ICommand DelBatchKeyCommand { get; private set; }//批量删除
        public ICommand AddKeyCommand { get; private set; }//添加
        public ICommand ClearKeyCommand { get; private set; }//清除

        #endregion

        #region 构造方法

        public PropertiesParseWindowViewModel()
        {
            Title = "解析Properties-CCT通用配置工具";
        }

        public PropertiesParseWindowViewModel(LoadedFile currentFile)
        {
            Title = "解析Properties-CCT通用配置工具";

            this.currentFile = currentFile;

            propertiesHelper = new PropertiesHelper(currentFile.FilePath);

            ExportExcelCommand = new DelegateCommand(ExportExcelCommandExecute);
            SaveCommand = new DelegateCommand(SaveCommandExecute);

            SelectAllKeyCommand = new DelegateCommand(SelectAllKeyCommandExecute);
            DelKeyCommand = new DelegateCommand(DelKeyCommandExecute);
            DelBatchKeyCommand = new DelegateCommand(DelBatchKeyCommandExecute);
            AddKeyCommand = new DelegateCommand(AddKeyCommandExecute);
            ClearKeyCommand = new DelegateCommand(ClearKeyCommandExecute);

            PropertyItemList = new ObservableCollection<PropertyItem>();

            ParseFile();

            if (PropertyItemList.Count > 0)
            {
                CurrentPropertyItem = PropertyItemList[0];
            }
        }

        #endregion

        #region 执行

        /// <summary>
        /// 导出为Excel
        /// </summary>
        private void ExportExcelCommandExecute()
        {
            // 获取路径
            var path = FileDialogHelper.SaveAsFile("xlsx");

            IsLoading = true;

            if (string.IsNullOrWhiteSpace(path))
            {
                IsLoading = false;
                return;
            }

            if (propertiesHelper.ExportExcel(path,PropertyItemList.ToList()))
            {
                MessageBox.Show("已导出！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                IsLoading = false;
                return;
            }

            IsLoading = false;
            MessageBox.Show("导出异常！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void SaveCommandExecute()
        {
            if (propertiesHelper.Save(currentFile.FilePath,PropertyItemList.ToList()))
            {
                MessageBox.Show("已保存！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            MessageBox.Show("保存异常！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 全选
        /// </summary>
        private void SelectAllKeyCommandExecute()
        {
            var selectedlist = PropertyItemList.Where(x=>x.IsChecked); 
            if(selectedlist.Count() == PropertyItemList.Count)
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
        /// 删除
        /// </summary>
        private void DelKeyCommandExecute()
        {
            PropertyItemList?.Remove(CurrentPropertyItem);
            if(PropertyItemList.Count>0)
            {
                CurrentPropertyItem = PropertyItemList[0];
            }
            NotifyUI();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        private void DelBatchKeyCommandExecute()
        {
            if(PropertyItemList.Count(x=>x.IsChecked)==0)
            {
                MessageBox.Show("请至少选择一项！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            for(int i=0;i<PropertyItemList.Count;i++)
            {
                var item = PropertyItemList[i];
                if (item.IsChecked)
                {
                    PropertyItemList.Remove(item);
                    i = -1;
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
                    PropertyItem newItem = new PropertyItem(this)
                    {
                        KeyName = keyName
                    };
                    PropertyItemList.Add(newItem);
                    NotifyUI();
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
            MessageBoxResult result = MessageBox.Show("确定要一键清除吗？","信息提示",MessageBoxButton.YesNoCancel,MessageBoxImage.Information);
            if (result==MessageBoxResult.Yes)
            {
                PropertyItemList.Clear();
            }
            NotifyUI();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 解析文件
        /// </summary>
        private void ParseFile()
        {
            foreach(var key in propertiesHelper.Keys)
            {
                PropertyItemList.Add(new PropertyItem(this)
                {
                     KeyName=key.ToString(),
                     KeyValue= propertiesHelper[key.ToString()].ToString()
                });
            }
            AddFilter();
        }

        /// <summary>
        /// 通知Converter变化
        /// </summary>
        public void NotifyUI()
        {    
            RaisePropertyChanged(nameof(PropertyItemList));
            RaisePropertyChanged(nameof(CurrentPropertyItem));
        }

        /// <summary>
        /// 添加过滤器
        /// </summary>
        private void AddFilter()
        {
            FilterPropertyItemListView.Filter += (obj) =>
            {
                if (string.IsNullOrWhiteSpace(searchText)) return true;

                var key = (obj as PropertyItem).KeyName;

                if(key.ToLower().Contains(searchText.ToLower()))
                {
                    return true;
                }

                return false;
            };
        }

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
