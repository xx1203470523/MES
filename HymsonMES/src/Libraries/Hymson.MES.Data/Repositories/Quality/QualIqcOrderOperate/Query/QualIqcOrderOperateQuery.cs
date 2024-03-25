using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// iqc检验单操作表 查询参数
    /// </summary>
    public class QualIqcOrderOperateQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public long? IQCOrderId { get; set; }

        /// <summary>
        /// 主键集合
        /// </summary>
        public IEnumerable<long>? IQCOrderIds { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public OrderOperateTypeEnum? OperationType { get; set; }

    }

}
