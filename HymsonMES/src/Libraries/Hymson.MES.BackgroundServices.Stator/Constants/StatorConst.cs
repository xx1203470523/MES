namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 常量（定子线）
    /// </summary>
    public class StatorConst
    {
        /// <summary>
        /// 工序前缀
        /// </summary>
        public const string PRODUCRE_PREFIX = "S01";

        /// <summary>
        /// 工序前缀
        /// </summary>
        public const string BUZ_KEY_PREFIX = "Stator";

        /// <summary>
        /// 定子部件有效前缀
        /// </summary>
        public const string BARCODE_PREFIX = "CSCW188";

        /// <summary>
        /// 默认值
        /// </summary>
        public const string USER = "LMS";

        /// <summary>
        /// 数量
        /// </summary>
        public const decimal QTY = 1;

        /// <summary>
        /// 忽略的字符串
        /// </summary>
        public static string[] IgnoreString = new string[] { "-", "_" };

    }
}
