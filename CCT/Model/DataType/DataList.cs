using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Model.DataType
{
    /// <summary>
    /// 数据源类
    /// </summary>
    public class DataList
    {
        /// <summary>
        /// InI
        /// </summary>
        public List<SectionItem> SelectionItemList = new List<SectionItem>();

        /// <summary>
        /// Properties
        /// </summary>
        public List<PropertyItem> PropertyItemList = new List<PropertyItem>();
    }
}
