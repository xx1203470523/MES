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
}
