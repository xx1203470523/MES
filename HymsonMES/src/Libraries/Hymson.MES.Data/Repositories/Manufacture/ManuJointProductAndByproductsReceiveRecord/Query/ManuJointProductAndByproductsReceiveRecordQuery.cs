namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 联副产品收货 查询参数
    /// </summary>
    public class ManuJointProductAndByproductsReceiveRecordQuery
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long[]? procMaterialIds { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>

        public long WorkOrderid { get; set; }
    }
}