namespace Hymson.MES.HttpClients.Options
{
    /// <summary>
    /// wms操作配置
    /// </summary>
    public class WMSOptions
    {
        /// <summary>
        /// 基础地址
        /// </summary>
        public string BaseAddressUri { get; set; } = "";

        /// <summary>
        /// 请求token
        /// </summary>
        public string SysToken { get; set; } = "";

        /// <summary>
        /// WMS回调路径（来料）
        /// </summary>
        public string IQCReceiptRoute { get; set; } = "";

        /// <summary>
        /// WMS回调路径（退料）
        /// </summary>
        public string IQCReturnRoute { get; set; } = "";

    }
}
