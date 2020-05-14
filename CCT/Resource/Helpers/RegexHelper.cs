namespace CCT.Resource.Helpers
{
    /// <summary>
    /// 正则表达式帮助类
    /// </summary>
    public class RegexHelper
    {
        #region IsPhoneNumber(是否合法的手机号码)
        /// <summary>
        /// 是否合法的手机号码
        /// </summary>
        /// <param name="value">手机号码</param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            return System.Text.RegularExpressions.Regex.IsMatch(value,@"^(0|86|17951)?(13[0-9]|15[012356789]|18[0-9]|14[57]|17[678])[0-9]{8}$");
        }
        #endregion

        #region IsMobileNumber(是否手机号码)
        /// <summary>
        /// 是否手机号码
        /// </summary>
        /// <param name="value">手机号码</param>
        /// <param name="isRestrict">是否按严格模式验证</param>
        /// <returns></returns>
        public static bool IsMobileNumberSimple(string value, bool isRestrict = false)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            string pattern = isRestrict ? @"^[1][3-8]\d{9}$" : @"^[1]\d{10}$";

            return System.Text.RegularExpressions.Regex.IsMatch(value, pattern);
        }
        /// <summary>
        /// 是否手机号码
        /// </summary>
        /// <param name="value">手机号码</param>
        /// <returns></returns>
        public static bool IsMobileNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            value = value.Trim().Replace("^", "").Replace("$", "");
            /**
             * 手机号码: 
             * 13[0-9], 14[5,7], 15[0, 1, 2, 3, 5, 6, 7, 8, 9], 17[6, 7, 8], 18[0-9], 170[0-9]
             * 移动号段: 134,135,136,137,138,139,150,151,152,157,158,159,182,183,184,187,188,147,178,1705
             * 联通号段: 130,131,132,155,156,185,186,145,176,1709
             * 电信号段: 133,153,180,181,189,177,1700
             */
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^1(3[0-9]|4[57]|5[0-35-9]|8[0-9]|70)\d{8}$");
        }

        /// <summary>
        /// 是否存在手机号码
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="isRestrict">是否按严格模式验证</param>
        /// <returns></returns>
        public static bool HasMobileNumberSimple(string value, bool isRestrict = false)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            string pattern = isRestrict ? @"[1][3-8]\d{9}" : @"[1]\d{10}";
            return System.Text.RegularExpressions.Regex.IsMatch(value, pattern);
        }
        #endregion

        #region IsChinaMobilePhone(是否中国移动号码)
        /// <summary>
        /// 是否中国移动号码
        /// </summary>
        /// <param name="value">手机号码</param>
        /// <returns></returns>
        public static bool IsChinaMobilePhone(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            /**
             * 中国移动：China Mobile
             * 134,135,136,137,138,139,150,151,152,157,158,159,182,183,184,187,188,147,178,1705
             */
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"(^1(3[4-9]|4[7]|5[0-27-9]|7[8]|8[2-478])\d{8}$)|(^1705\d{7}$)");
        }
        #endregion

        #region IsChinaUnicomPhone(是否中国联通号码)
        /// <summary>
        /// 是否中国联通号码
        /// </summary>
        /// <param name="value">手机号码</param>
        /// <returns></returns>
        public static bool IsChinaUnicomPhone(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            /**
             * 中国联通：China Unicom
             * 130,131,132,155,156,185,186,145,176,1709
             */
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"(^1(3[0-2]|4[5]|5[56]|7[6]|8[56])\d{8}$)|(^1709\d{7}$)");
        }
        #endregion

        #region IsChinaTelecomPhone(是否中国电信号码)
        /// <summary>
        /// 是否中国电信号码
        /// </summary>
        /// <param name="value">手机号码</param>
        /// <returns></returns>
        public static bool IsChinaTelecomPhone(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            /**
             * 中国电信：China Telecom
             * 133,153,180,181,189,177,1700
             */
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"(^1(33|53|77|8[019])\d{8}$)|(^1700\d{7}$)");
        }
        #endregion

        #region 是否为数字

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumber(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^[0-9]*$");
        }

        #endregion
    }
}
