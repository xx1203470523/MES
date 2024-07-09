namespace Hymson.MES.Data.Options
{
    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public partial class ConnectionOptions
    {
        /// <summary>
        /// 连接字符串（转子线）
        /// </summary>
        public string RotorConnectionString { get; set; } = "";

        /// <summary>
        /// 连接字符串（定子线）
        /// </summary>
        public string StatorConnectionString { get; set; } = "";

    }
}
