using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// Iqc检验详情 分页参数
    /// </summary>
    public class QualIqcOrderSampleDetailPagedQuery : PagerInfo
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
        /// 检验类型ID
        /// </summary>
        public long? IQCOrderTypeId { get; set; }

        /// <summary>
        /// ID集合（样品）
        /// </summary>
        public IEnumerable<long>? IQCOrderSampleIds { get; set; }

        /// <summary>
        /// ID集合（快照明细）
        /// </summary>
        public IEnumerable<long>? IQCInspectionDetailSnapshotIds { get; set; }

    }
}
