using System.Collections.ObjectModel;

namespace CCT.Model.DataType
{
    public class Node:BaseModel
    {
        #region 私有域

        private string displayName;//显示名称

        private string nodeType;//节点类型

        private bool isExpand = false;//是否展开

        private bool canRemove = true;//是否可从父节点移除

        private bool canInsert = true;//是否可插入子节点

        private Node parent;//父节点

        #endregion

        #region 属性

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set { SetProperty(ref displayName, value); }
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        public string NodeType
        {
            get { return nodeType; }
            set { SetProperty(ref nodeType, value); }
        }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpand
        {
            get { return isExpand; }
            set { SetProperty(ref isExpand, value); }
        }

        /// <summary>
        /// 是否可从父节点移除
        /// </summary>
        public bool CanRemove
        {
            get { return canRemove; }
            set { SetProperty(ref canRemove, value); }
        }

        /// <summary>
        /// 是否可插入子节点
        /// </summary>
        public bool CanInsert
        {
            get { return canInsert; }
            set { SetProperty(ref canInsert, value); }
        }

        /// <summary>
        /// 父节点
        /// </summary>
        public Node Parent
        {
            get { return parent; }
            set { SetProperty(ref parent, value); }
        }

        /// <summary>
        /// 孩子节点
        /// </summary>
        public ObservableCollection<Node> Children { get; set; }

        /// <summary>
        /// 节点属性
        /// </summary>
        public ObservableCollection<PropertyItem> Attributies { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 默认
        /// </summary>
        public Node()
        {
            Children = new ObservableCollection<Node>();
            Attributies = new ObservableCollection<PropertyItem>();
        }

        #endregion
    }
}
