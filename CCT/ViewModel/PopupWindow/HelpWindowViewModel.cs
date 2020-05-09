using CCT.Resource.Dictionarys;

namespace CCT.ViewModel
{
    public class HelpWindowViewModel:ViewModelBase
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

        public HelpWindowViewModel()
        {
            Version = ResourceHelper.FindStringResource("Version");
        }

        #endregion
    }
}
