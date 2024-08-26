namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 生产领料单 查询参数
    /// </summary>
    public class ManuRequistionOrderQuery
    {

        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        // <summary>
        /// 领料单据号
        /// </summary>
        public string ReqOrderCode { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? WorkOrderCode { get; set; }

    }

    /// <summary>
    /// 根据工单集合查询领料单
    /// </summary>
    public class ManuRequistionQueryByWorkOrders
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<long>? WorkOrderIds { get; set; }

        /// <summary>
        /// 领料的同步单号
        /// </summary>
        public string? ReqOrderCode { get; set; }

    }
}
