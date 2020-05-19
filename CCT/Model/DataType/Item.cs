using System.Xml.Serialization;

namespace CCT.Model.DataType
{
    public class Item
    {
        [XmlAttribute("X1")]
        public string X1 { get; set; }
        [XmlAttribute("X2")]
        public string X2 { get; set; }
    }
}
