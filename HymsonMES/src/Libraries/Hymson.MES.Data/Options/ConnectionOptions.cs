namespace Hymson.MES.Data.Options
{
    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public partial class ConnectionOptions
    {
        /// <summary>
        /// MES连接字符串
        /// </summary>
        public string MESConnectionString { get; set; } = "";

        /// <summary>
        /// MES参数数据库连接
        /// </summary>
        public string MESParamterConnectionString { get; set; } = "";

        /// <summary>
        /// Doris参数数据库连接
        /// </summary>
        public string DorisParamterConnectionString { get; set; } = "";

    }
}
