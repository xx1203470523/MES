namespace Hymson.MES.Core.Constants.Manufacture
{
    /// <summary>
    /// 生成条码通配符
    /// </summary>
    public static class GenerateBarcodeWildcard
    {
        /// <summary>
        /// 流水
        /// </summary>
        public const string Activity = "%ACTIVITY%";

        /// <summary>
        /// 日期
        /// </summary>
        public const string Yymmdd = "%YYMMDD%";

        /// <summary>
        /// 
        /// </summary>
        public const string MultipleVariable = "%MULTIPLE_VARIABLE%";

        /// <summary>
        /// 年月日通配符
        /// </summary>
        public const string YMDWildcard = "%YMD_WILDCARD%";

        /// <summary>
        /// 线别通配符
        /// </summary>
        public const string LINETYPE = "%LINE_TYPE%";
    }
}
