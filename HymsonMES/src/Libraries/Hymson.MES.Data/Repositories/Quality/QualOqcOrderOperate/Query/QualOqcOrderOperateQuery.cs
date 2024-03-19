using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// OQC检验单操作记录 查询参数
    /// </summary>
    public class QualOqcOrderOperateQuery
    {
        /// <summary>
        /// OQC检验单Id
        /// </summary>
        public long? OQCOrderId { get; set; }

        /// <summary>
        /// OQC检验单Ids
        /// </summary>
        public IEnumerable<long>? OQCOrderIds { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 单据状态
        /// </summary>
        public OrderOperateTypeEnum? OperationType { get; set; }

        /// <summary>
        /// 单据状态s
        /// </summary>
        public IEnumerable<OrderOperateTypeEnum>? OperationTypes { get; set; }
    }
}
