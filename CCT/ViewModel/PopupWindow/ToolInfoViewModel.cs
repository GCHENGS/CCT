namespace CCT.ViewModel
{
    /// <summary>
    /// 工具简介视图模型
    /// </summary>
    public class ToolInfoViewModel : ViewModelBase
    {
        #region 私有域

        #endregion

        #region 公有属性

        /// <summary>
        /// 工具简介
        /// </summary>
        public string ToolInfo { get; set; }

        #endregion

        #region 构造方法

        public ToolInfoViewModel()
        {
            Title = "工具简介-通用配置工具(CCT)";
            ToolInfo = "CCT 是一款可以在不同配置文件之间进行处理和转换的工具软件。";
        }

        #endregion
    }
}
