using CCT.Model.DataType;
using CCT.Service;
using Prism.Commands;
using System.Windows;
using System.Windows.Input;

namespace CCT.ViewModel
{
    /// <summary>
    /// 修改密码视图模型
    /// </summary>
    public class UpdatePwdViewModel : ViewModelBase
    {
        #region 私有域

        private User currentUser;//当前用户
        private string oldPassword;//旧密码
        private string newPassword;//新密码

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
        /// 旧密码
        /// </summary>
        public string OldPassword
        {
            get { return oldPassword; }
            set { SetProperty(ref oldPassword, value); }
        }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword
        {
            get { return newPassword; }
            set { SetProperty(ref newPassword, value); }
        }

        #endregion

        #region 命令

        public ICommand UpdatePwdCommand { get; private set; }//修改密码

        #endregion

        #region 构造方法

        public UpdatePwdViewModel(User user)
        {
            CurrentUser = user;
            Title = CurrentUser.UserName + "用户，你正在修改密码";
            UpdatePwdCommand = new DelegateCommand<Window>(UpdatePwdCommandExecute);
        }
        #endregion

        #region 执行

        /// <summary>
        /// 修改密码
        /// </summary>
        private void UpdatePwdCommandExecute(Window win)
        {
            if(string.IsNullOrWhiteSpace(oldPassword))
            {
                MessageBox.Show("请先输入旧密码...","信息提示",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }
            if(!currentUser.UserPassword.Equals(oldPassword))
            {
                MessageBox.Show("旧密码不正确，请重新输入旧密码...","信息提示",MessageBoxButton.OK,MessageBoxImage.Warning);
                OldPassword = string.Empty;
                return;
            }
            else
            {
                if(string.IsNullOrWhiteSpace(newPassword))
                {
                    MessageBox.Show("请输入新密码...", "信息提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if(newPassword.Equals(oldPassword))
                {
                    MessageBox.Show("新旧密码一致，请重新输入新密码...", "信息提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NewPassword = string.Empty;
                    return;
                }
                
                if(UserService.UpdatePwd(newPassword,currentUser.UserId))
                {
                    MessageBox.Show("更新成功!", "信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    OldPassword = string.Empty;
                    NewPassword = string.Empty;
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
