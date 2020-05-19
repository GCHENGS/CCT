using CCT.Model.DataType;

namespace CCT.Model.InterFace
{
    public interface IDefinitions
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        SysConfig SysConfig { get; }

        /// <summary>
        /// 读取所有的预定义Xml文件
        /// </summary>
        /// <returns></returns>
        bool ReadDefinitions(string xmlPath);

        /// <summary>
        /// 保存所有的预定义Xml文件
        /// </summary>
        /// <returns></returns>
        bool SaveDefinitions(string xmlPath,SysConfig sysConfig);
    }
}
