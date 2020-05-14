using CCT.Resource.Dictionarys;

namespace CCT.ViewModel
{
    /// <summary>
    /// 帮助视图模型
    /// </summary>
    public class HelpViewModel : ViewModelBase
    {
        #region 私有域

        private string version = string.Empty;

        #endregion

        #region 公有属性

        public string Version
        {
            get { return version; }
            set { SetProperty(ref version, value); }
        }

        #endregion

        #region 构造方法

        public HelpViewModel()
        {
            Version = ResourceHelper.FindStringResource("Version");
        }

        #endregion
    }
}
