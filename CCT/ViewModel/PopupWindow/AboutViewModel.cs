namespace CCT.ViewModel
{
    /// <summary>
    /// 关于视图模型
    /// </summary>
    public class AboutViewModel : ViewModelBase
    {
        #region 私有域

        #endregion

        #region 公有属性

        /// <summary>
        /// 产生背景
        /// </summary>
        public string BackGround { get; set; }

        #endregion

        #region 构造方法

        public AboutViewModel()
        {
            Title = "关于-通用配置工具(CCT)";
            BackGround = "目前的绝大多数系统都依赖配置文件的支持，如果没有这些底层的配置文件，系统便无法正常运行。而对于拥有大量配置文件的系统，如果要更新和转换系统的配置文件内容无疑是一件十分繁琐的事情。";
        }

        #endregion
    }
}
