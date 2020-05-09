using CCT.Resource.Dictionarys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Resource.Constants
{
    public static class ConstantsForString
    {
        //Zh/En
        public static object Zh => ResourceHelper.FindStringResource("Zh");
        public static object En => ResourceHelper.FindStringResource("En");
        public static object Zh_CN => ResourceHelper.FindStringResource("Zh_CN");
        public static object En_US => ResourceHelper.FindStringResource("En_US");

        //弹窗

    }
}
