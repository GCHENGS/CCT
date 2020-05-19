using CCT.Model.DataType;
using CCT.Resource.Helpers;
using CCT.Service;
using Prism.Commands;
using System.Windows;
using System.Windows.Input;

namespace CCT.ViewModel
{
    /// <summary>
    /// 修改电话视图模型
    /// </summary>
    public class UpdateTelViewModel:ViewModelBase
    {
        #region 私有域

        private User currentUser;//当前用户
        private string newUserPhone;//新电话

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

        /// <summary>
        /// 新电话
        /// </summary>
        public string NewUserPhone
        {
            get { return newUserPhone; }
            set { SetProperty(ref newUserPhone, value); }
        }

        #endregion

        #region 命令

        public ICommand UpdateTelCommand { get; private set; }//修改电话

        #endregion

        #region 构造方法

        public UpdateTelViewModel(User user)
        {
            CurrentUser = user;
            Title = CurrentUser.UserName + "用户，你正在修改电话";
            UpdateTelCommand = new DelegateCommand<Window>(UpdateTelCommandExecute);
        }
        #endregion

        #region 执行

        /// <summary>
        /// 修改电话
        /// </summary>
        private void UpdateTelCommandExecute(Window win)
        {
            if (string.IsNullOrWhiteSpace(newUserPhone))
            {
                MessageBox.Show("请输入电话...", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (currentUser.UserPhone.Equals(newUserPhone))
            {
                MessageBox.Show("新旧电话一样，请重新输入...", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                NewUserPhone = string.Empty;
                return;
            }
            else
            {

                if(!RegexHelper.IsPhoneNumber(newUserPhone))
                {
                    MessageBox.Show("非法手机号，请重新输入...", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    NewUserPhone = string.Empty;
                    return;
                }

                if (UserService.UpdateTel(newUserPhone, currentUser.UserId))
                {
                    MessageBox.Show("更新成功!", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    win.Close();
                }
                else
                {
                    MessageBox.Show("更新失败，请稍后重试!", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        #endregion
    }
}
