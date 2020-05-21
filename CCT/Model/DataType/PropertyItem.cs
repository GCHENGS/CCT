using CCT.ViewModel;
using Prism.Commands;
using System.Windows.Input;

namespace CCT.Model.DataType
{
    /// <summary>
    /// 键-值配置类
    /// </summary>
    public class PropertyItem : BaseModel
    {

        #region 私有域

        private string keyName=string.Empty;// 键

        private string keyValue=string.Empty;//值

        private bool isChecked=false;//是否选中

        #endregion

        #region 属性

        /// <summary>
        /// 关联视图模型-properties
        /// </summary>
        public PropertiesParseWindowViewModel PropertiesParseWindowViewModel { get; set; }

        /// <summary>
        /// 关联视图模型-ini
        /// </summary>
        public InIParseWindowViewModel InIParseWindowViewModel { get; set; }

        /// <summary>
        /// 关联视图模型-xml
        /// </summary>
        public XmlParseWindowViewModel XmlParseWindowViewModel { get; set; }

        /// <summary>
        /// 关联树节点
        /// </summary>
        public Node TreeNode { get; set; }

        /// <summary>
        /// 键名称
        /// </summary>
        public string KeyName
        {
            get { return keyName; }
            set { SetProperty(ref keyName, value); }
        }

        /// <summary>
        /// 键-值
        /// </summary>
        public string KeyValue
        {
            get { return keyValue; }
            set { SetProperty(ref keyValue, value); }
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                SetProperty(ref isChecked, value);
            }
        }

        #endregion

        #region 命令

        public ICommand DelCommand { get; private set; }//删除

        #endregion

        #region 构造方法

        public PropertyItem()
        {

        }

        /// <summary>
        /// properties
        /// </summary>
        /// <param name="propertiesParseWindowViewModel"></param>
        public PropertyItem(PropertiesParseWindowViewModel propertiesParseWindowViewModel)
        {
            PropertiesParseWindowViewModel = propertiesParseWindowViewModel;
            DelCommand = new DelegateCommand(DelPropertiesCommandExecute);
        }

        /// <summary>
        /// ini
        /// </summary>
        /// <param name="iniParseWindowViewModel"></param>
        public PropertyItem(InIParseWindowViewModel iniParseWindowViewModel)
        {
            InIParseWindowViewModel = iniParseWindowViewModel;
            DelCommand = new DelegateCommand(DelInICommandExecute);
        }

        /// <summary>
        /// xml
        /// </summary>
        /// <param name="node"></param>
        public PropertyItem(Node node,XmlParseWindowViewModel xmlParseWindowViewModel)
        {
            TreeNode = node;
            XmlParseWindowViewModel = xmlParseWindowViewModel;
            DelCommand = new DelegateCommand(DelXmlCommandExecute);
        }

        #endregion

        #region 执行

        /// <summary>
        /// 删除-properties
        /// </summary>
        private void DelPropertiesCommandExecute()
        {
            PropertiesParseWindowViewModel?.PropertyItemList.Remove(this) ;
            PropertiesParseWindowViewModel?.NotifyUI();
        }

        /// <summary>
        /// 删除-ini
        /// </summary>
        private void DelInICommandExecute()
        {
            InIParseWindowViewModel?.PropertyItemList.Remove(this);
            InIParseWindowViewModel?.CurrentSectionItem?.PropertyItems.Remove(this);
            InIParseWindowViewModel?.NotifyUI();
        }

        /// <summary>
        /// 删除-xml
        /// </summary>
        private void DelXmlCommandExecute()
        {       
            TreeNode?.Attributies.Remove(this);
            XmlParseWindowViewModel?.PropertyItemList.Remove(this);
            XmlParseWindowViewModel?.NotifyUI();
        }

        #endregion
    }
}
