namespace CCT.Resource.Enums
{
    /// <summary>
    /// 多绑定输出格式
    /// </summary>
    public enum MultiBindingFormatType
    {
        /// <summary>
        /// 一般属性格式：A（B）
        /// </summary>
        CommonFormat,
        /// <summary>
        /// 带下级属性格式：A(C) : B(D)
        /// </summary>
        CommonWithSubFormat,
        /// <summary>
        /// ini配置节格式：[A]
        /// </summary>
        SectionFormat
    }
}
