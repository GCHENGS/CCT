using CCT.Model.DataType;

namespace CCT.ViewModel
{
    /// <summary>
    /// 个人信息视图模型
    /// </summary>
    public class PersonViewModel:ViewModelBase
    {
        #region 私有域

        private User currentUser;//当前用户

        #endregion

        #region  属性

        /// <summary>
        /// 当前用户
        /// </summary>
        public User CurrentUser
        {
            get { return currentUser; }
            set { SetProperty(ref currentUser, value); }
        }

        #endregion

        #region 构造方法

        public PersonViewModel(User user)
        {
            CurrentUser = user;
        }

        #endregion
    }
}
