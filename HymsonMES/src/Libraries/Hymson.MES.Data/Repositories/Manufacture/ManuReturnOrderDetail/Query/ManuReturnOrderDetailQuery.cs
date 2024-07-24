namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 生产退料单明细 查询参数
    /// </summary>
    public class ManuReturnOrderDetailQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 退料单ID
        /// </summary>
        public long? ReturnOrderId { get; set; }

        /// <summary>
        /// 退料单Id列表
        /// </summary>
        public IEnumerable<long>? ReturnOrderIds { get; set; }

    }
}
