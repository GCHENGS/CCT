using CCT.Resource.Dictionarys;

namespace CCT.ViewModel
{
    public class ContactWindowViewModel:ViewModelBase
    {
        #region 私有域

        private string phone = string.Empty;
        private string e_mail = string.Empty;

        #endregion

        #region 公有属性

        public string Phone
        {
            get { return phone; }
            set { SetProperty(ref phone, value); }
        }

        public string E_mail
        {
            get { return e_mail; }
            set { SetProperty(ref e_mail, value); }
        }

        #endregion

        #region 构造方法

        public ContactWindowViewModel()
        {
            Phone = ResourceHelper.FindStringResource("Phone");
            E_mail = ResourceHelper.FindStringResource("E_mail");
        }

        #endregion
    }
}
