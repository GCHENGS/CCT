using System.Collections.Generic;
using System.Xml.Serialization;

namespace CCT.Model.DataType
{
    public class SavedRecentFile
    {
        [XmlArray(ElementName="List")]
        [XmlArrayItem("RecentFile")]
        public List<RecentFile> List = new List<RecentFile>();
    }
}
