using CCT.Resource.Constants;
using CCT.View;
using CCT.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CCT
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void OnAppStartup_UpdateThemeName(object sender, StartupEventArgs e)
        {
            DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName();
        }
        public static string Language { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            GetLanguage();

            var window = new MainWindow()
            {
                DataContext = new MainWindowViewModel()
            };
            window.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SaveLanguage();
            //关闭所有线程，即关闭此进程
            System.Environment.Exit(0);
        }

        #region Method

        /// <summary>
        /// 开机启动默认的语言
        /// </summary>
        private void GetLanguage()
        {
            Language = string.Empty;

            try
            {
                Language = CCT.Properties.Settings.Default.Language.Trim();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            var s = ConstantsForString.En_US.ToString();

            Language = string.IsNullOrEmpty(Language) ? ConstantsForString.En_US.ToString() : Language;

            //update Language
            UpdateLanguage();
        }

        /// <summary>
        /// 保存语言设置
        /// </summary>
        private void SaveLanguage()
        {
            try
            {
                CCT.Properties.Settings.Default.Language = Language;
                CCT.Properties.Settings.Default.Save();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
             }
        }

        /// <summary>
        /// 更换语言包
        /// </summary>
        public static void UpdateLanguage()
        {
            List<ResourceDictionary> dictionaryList = new List<ResourceDictionary>();
            foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
            {
                var S = dictionary.Source.OriginalString;
                dictionaryList.Add(dictionary);
            }

            string requestedLanguage = string.Format(@"pack://application:,,,/Resource/Dictionarys/StringResource/{0}.xaml", Language);
            ResourceDictionary resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals(requestedLanguage));

            if (resourceDictionary == null)
            {
                requestedLanguage = @"pack://application:,,,/Resource/Dictionarys/StringResource/En_US.xaml";
                resourceDictionary = dictionaryList.FirstOrDefault(d => d.Source.OriginalString.Equals(requestedLanguage));
            }

            if (resourceDictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }
        }

        #endregion
    }
}
