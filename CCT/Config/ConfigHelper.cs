using CCT.Model.DataType;

namespace CCT.Config
{
    public class ConfigHelper
    {
        private static Definitions df = new Definitions();
        private static string configPath = "D:/VS开发/CCT/CCT/Config";

        /// <summary>
        /// 读取系统配置
        /// </summary>
        public static SysConfig ReadSysConfig()
        {
            if(df.ReadDefinitions(configPath))
            {
                return df.SysConfig;
            }
            return null;
        }

        /// <summary>
        /// 保存系统配置
        /// </summary>
        public static bool SaveSysConfig(SysConfig sysConfig)
        {
            if(df.SaveDefinitions(configPath, sysConfig))
            {
                return true;
            }
            return false;
        }
    }
}
