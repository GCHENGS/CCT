using CCT.Model.DataType;
using CCT.Resource.Enums;
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
    public class InIParseWindowViewModel : ViewModelBase
    {
        #region 私有域

        private ViewModelStatus _status = ViewModelStatus.None;//异步状态

        private string searchText;//搜索字符串

        private LoadedFile currentFile;//当前文件

        private InIHelper iniHelper;//属性文件助手

        private SectionItem currentSectionItem;//当前节

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

                FilterSectionItemListView?.Refresh();

                NotifyUI();
            }
        }

        /// <summary>
        /// section数据源
        /// </summary>
        public ObservableCollection<SectionItem> SectionItemList { get; set; }

        /// <summary>
        /// 映射View
        /// </summary>
        public ICollectionView FilterSectionItemListView
        {
            get
            {
                return CollectionViewSource.GetDefaultView(SectionItemList);
            }
        }

        /// <summary>
        /// key数据源
        /// </summary>
        public ObservableCollection<PropertyItem> PropertyItemList { get; set; }

        /// <summary>
        /// 当前section
        /// </summary>
        public SectionItem CurrentSectionItem
        {
            get { return currentSectionItem; }
            set
            {
                if (value == null) return;
                SetProperty(ref currentSectionItem, value);
                if(currentSectionItem!=null)
                {
                    PropertyItemList.Clear();
                    PropertyItemList.AddRange(currentSectionItem.PropertyItems);
                    NotifyUI();
                }
            }
        }

        /// <summary>
        /// 是否显示加载
        /// </summary>
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value);  }
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

        #endregion

        #region 命令

        public ICommand ExportExcelCommand { get; private set; }//导出
        public ICommand SaveCommand { get; private set; }//保存

        public ICommand AddSectionCommand { get; private set; }//添加节
        public ICommand DelSectionCommand { get; private set; }//删除节
        public ICommand ClearSectionCommand { get; private set; }//清除节  
            
        public ICommand SelectAllKeyCommand { get; private set; }//全选
        public ICommand DelBatchKeyCommand { get; private set; }//批量删除
        public ICommand AddKeyCommand { get; private set; }//添加
        public ICommand ClearKeyCommand { get; private set; }//清除

        #endregion

        #region 构造方法

        public InIParseWindowViewModel()
        {
            Title = "解析InI-CCT通用配置工具";
        }

        public InIParseWindowViewModel(LoadedFile currentFile)
        {
            Title = "解析InI-CCT通用配置工具";

            this.currentFile = currentFile;

            iniHelper = new InIHelper(currentFile.FilePath);

            ExportExcelCommand = new DelegateCommand(ExportExcelCommandExecute);
            SaveCommand = new DelegateCommand(SaveCommandExecute);

            AddSectionCommand = new DelegateCommand(AddSectionCommandExecute);
            DelSectionCommand = new DelegateCommand(DelSectionCommandExecute);
            ClearSectionCommand = new DelegateCommand(ClearSectionCommandExecute);

            SelectAllKeyCommand = new DelegateCommand(SelectAllKeyCommandExecute);
            DelBatchKeyCommand = new DelegateCommand(DelBatchKeyCommandExecute);
            AddKeyCommand = new DelegateCommand(AddKeyCommandExecute);
            ClearKeyCommand = new DelegateCommand(ClearKeyCommandExecute);

            SectionItemList = new ObservableCollection<SectionItem>();

            PropertyItemList = new ObservableCollection<PropertyItem>();

            ParseFile();

            if (SectionItemList.Count > 0)
            {
                CurrentSectionItem = SectionItemList[0];//默认选中第一个
            }
        }

        #endregion

        #region 执行

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
            
            if (iniHelper.ExportExcel(path, SectionItemList.ToList()))
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
        /// 保存
        /// </summary>
        private void SaveCommandExecute()
        {
            if (iniHelper.Save(SectionItemList.ToList()))
            {
                MessageBox.Show("已保存！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            MessageBox.Show("保存异常！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 添加节
        /// </summary>
        private void AddSectionCommandExecute()
        { 
            //获取输入
            string sectionName = GetUserInput("注意：输入不能为空且不能重复！", "请输入Section名称","");
            if (!string.IsNullOrWhiteSpace(sectionName))
            {
                if (SectionItemList.Count(x => x.SectionName.Equals(sectionName)) == 0)
                {
                    SectionItem section = new SectionItem()
                    {
                        SectionName = sectionName
                    };
                    SectionItemList.Add(section);
                    CurrentSectionItem = section;
                    NotifyUI();
                }
                else
                {
                    MessageBox.Show("Section重复，创建失败！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            } 
        }

        /// <summary>
        /// 删除节
        /// </summary>
        private void DelSectionCommandExecute()
        {
            SectionItemList.Remove(CurrentSectionItem);
            if (SectionItemList.Count > 0)
            {
                CurrentSectionItem = SectionItemList[0];
            }
            NotifyUI();
        }

        /// <summary>
        /// 清除节
        /// </summary>
        private void ClearSectionCommandExecute()
        {
            MessageBoxResult result = MessageBox.Show("确定要一键清除吗？", "信息提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            { 
                SectionItemList.Clear();
                PropertyItemList.Clear();
            }
            NotifyUI();
        }

        /// <summary>
        /// 全选
        /// </summary>
        private void SelectAllKeyCommandExecute()
        {
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
                    PropertyItemList.Remove(item);
                    CurrentSectionItem?.PropertyItems.Remove(item);
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
                    CurrentSectionItem?.PropertyItems.Add(newItem);
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
            MessageBoxResult result = MessageBox.Show("确定要一键清除吗？", "信息提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                PropertyItemList.Clear();
                CurrentSectionItem?.PropertyItems.Clear(); 
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
            try
            {
                SectionItem sectionItem;
                //所有sections
                foreach (var section in iniHelper.GetSectionNames())
                {
                    sectionItem = new SectionItem() { SectionName = section };
                    //指定section下所有key
                    foreach (var key in iniHelper.GetKeys(section))
                    {
                        sectionItem.PropertyItems.Add(new PropertyItem(this)
                        {
                            KeyName = key,
                            KeyValue = iniHelper.GetValue(section, key)
                        });
                    }
                    SectionItemList.Add(sectionItem);
                }
            }
            catch
            {
                MessageBox.Show("解析异常！", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            AddFilter();
        }

        /// <summary>
        /// 通知Converter变化
        /// </summary>
        public void NotifyUI()
        {
            RaisePropertyChanged(nameof(SectionItemList));
            RaisePropertyChanged(nameof(PropertyItemList));
            RaisePropertyChanged(nameof(CurrentSectionItem));
        }

        /// <summary>
        /// 添加过滤器
        /// </summary>
        private void AddFilter()
        {
            FilterSectionItemListView.Filter += (obj) =>
            {
                var sectionName = (obj as SectionItem).SectionName;
                if (sectionName == currentSectionItem?.SectionName)
                {
                    CurrentSectionItem = (obj as SectionItem);//显示选中项
                }

                if (string.IsNullOrWhiteSpace(searchText)) return true;

                if (sectionName.ToLower().Contains(searchText.ToLower()))//模糊查询
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
        private string GetUserInput(string tip,string title,string def)
        {
            return Interaction.InputBox(tip, title, def, -1, -1);
        }

        #endregion
    }
}
