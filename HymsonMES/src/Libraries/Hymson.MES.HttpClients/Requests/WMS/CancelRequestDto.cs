namespace Hymson.MES.HttpClients.Requests.WMS
{
    /// <summary>
    /// 取消入库
    /// </summary>
    public class CancelEntryDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string SyncCode { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string UpdatedBy { get; set; } = "";

    }

    /// <summary>
    /// 取消出库
    /// </summary>
    public class CancelDeliveryDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string SyncCode { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string UpdatedBy { get; set; } = "";

    }

}
