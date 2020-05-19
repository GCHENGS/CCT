using CCT.Model.InterFace;
using System.IO;
using System.Xml.Serialization;

namespace CCT.Model.DataType
{
    public class Definitions:IDefinitions
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        public SysConfig SysConfig { get; private set; } = new SysConfig();

        /// <summary>
        /// 读取所有的预定义Xml文件
        /// </summary>
        /// <returns></returns>
        public bool ReadDefinitions(string xmlPath)
        {
            try
            {
                using (FileStream fs = new FileStream(xmlPath + @"\SysConfig.xml", FileMode.Open, FileAccess.Read))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SysConfig));
                    SysConfig = serializer.Deserialize(fs) as SysConfig;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保存所有的预定义Xml文件
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public bool SaveDefinitions(string xmlPath,SysConfig sysConfig)
        {
            try
            {
                using (StreamWriter sw=new StreamWriter(xmlPath+ @"\SysConfig.xml"))
                {
                    XmlSerializer serializer = new XmlSerializer(sysConfig.GetType(),new XmlRootAttribute("SysConfig"));
                    serializer.Serialize(sw, sysConfig);
                    sw.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
