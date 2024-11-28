namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 工单完工入库 查询参数
    /// </summary>
    public class ManuProductReceiptOrderQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 退料工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 成品入库单号
        /// </summary>
        public string? CompletionOrderCode { get; set; }

    }

    /// <summary>
    /// 取消入库
    /// </summary>
    public class ManuProductReceiptOrderByScwQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 退料工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 成品入库单号
        /// </summary>
        public string? CompletionOrderCode { get; set; }

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
}
