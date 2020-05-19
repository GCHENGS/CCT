using CCT.ViewModel;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CCT.Model.DataType
{
    /// <summary>
    /// InI 节点类
    /// </summary>
    public class SectionItem : BaseModel
    {

        #region 私有域

        private string sectionName = string.Empty;// 节名称
         
        #endregion

        #region 属性

        /// <summary>
        /// 关联视图模型
        /// </summary>
        public InIParseWindowViewModel InIParseWindowViewModel { get; set; }

        /// <summary>
        /// 节名称
        /// </summary>
        public string SectionName
        {
            get { return sectionName; }
            set { SetProperty(ref sectionName, value); }
        }

        /// <summary>
        /// 键-值列表
        /// </summary>
        public ObservableCollection<PropertyItem> PropertyItems = new ObservableCollection<PropertyItem>();

        #endregion

        #region 命令

        public ICommand DelCommand { get; private set; }//删除

        #endregion

        #region 构造方法

        public SectionItem()
        {

        }

        public SectionItem(InIParseWindowViewModel iniParseWindowViewModel)
        {
            InIParseWindowViewModel = iniParseWindowViewModel;

            DelCommand = new DelegateCommand(DelCommandExecute);
        }

        #endregion

        #region 执行

        /// <summary>
        /// 删除
        /// </summary>
        private void DelCommandExecute()
        {
            InIParseWindowViewModel?.SectionItemList.Remove(this);
            InIParseWindowViewModel?.NotifyUI();
        }

        #endregion
    }
}
