using System.Xml.Serialization;

namespace CCT.Model.DataType
{
    public class SysConfig
    {
        [XmlElement("SavedLastLoginUser")]
        public SavedLastLoginUser SavedLastLoginUser { get; set; } = new SavedLastLoginUser();

        [XmlElement("SaveUserOperator")]
        public SaveUserOperator SaveUserOperator { get; set; } = new SaveUserOperator();

        [XmlElement("SavedLastOpenFile")]
        public SavedLastOpenFile SavedLastOpenFile { get; set; } = new SavedLastOpenFile();

        [XmlElement("SavedRecentFile")]
        public SavedRecentFile SavedRecentFile { get; set; } = new SavedRecentFile();
    }
}
