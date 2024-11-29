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
    /// 取消入库
    /// </summary>
    public class CancelEntryByScwDto
    {
        /// <summary>
        /// 同步单号
        /// </summary>
        public string SyncCode { get; set; } = "";

        /// <summary>
        /// 唯一码
        /// </summary>
        public string UniqueCode { get; set; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// MES明细ID
        /// </summary>
        public long SyncId { get; set; }

        /// <summary>
        /// 操作人
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
