using CCT.Resource.Dictionarys;

namespace CCT.Resource.Constants
{
    public static class ConstantsForString
    {
        //Zh/En
        public static object Zh => ResourceHelper.FindStringResource("Zh");
        public static object En => ResourceHelper.FindStringResource("En");
        public static object Zh_CN => ResourceHelper.FindStringResource("Zh_CN");
        public static object En_US => ResourceHelper.FindStringResource("En_US");

        //窗体标题
        public static object MainWindowTitle => ResourceHelper.FindStringResource("MainWindowTitle");
        public static object _MainWindowTitle => ResourceHelper.FindStringResource("-MainWindowTitle");

        //窗体内容
        public static object Ready => ResourceHelper.FindStringResource("Ready");

        //菜单
        public static object Help => ResourceHelper.FindStringResource("Help");
        public static object UserFeedBack => ResourceHelper.FindStringResource("UserFeedBack");
        public static object ContactUs => ResourceHelper.FindStringResource("ContactUs");
        public static object About => ResourceHelper.FindStringResource("About");
    }
}
