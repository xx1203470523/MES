using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// FQC检验单操作记录 查询参数
    /// </summary>
    public class QualFqcOrderOperateQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public long? FQCOrderId { get; set; }

        /// <summary>
        /// 主键集合
        /// </summary>
        public IEnumerable<long>? FQCOrderIds { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public OrderOperateTypeEnum? OperationType { get; set; }
    }
}
