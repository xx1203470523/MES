using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（Iqc检验详情）   
    /// qual_iqc_order_sample_detail
    /// @author Czhipu
    /// @date 2024-03-06 02:26:39
    /// </summary>
    public class QualIqcOrderSampleDetailEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// IQC检验单Id
        /// </summary>
        public long IQCOrderId { get; set; }

        /// <summary>
        /// IQC检验单样本Id
        /// </summary>
        public long IQCOrderSampleId { get; set; }

        /// <summary>
        /// qual_iqc_inspection_item_detail_snapshot的Id
        /// </summary>
        public long IQCInspectionDetailSnapshotId { get; set; }

        /// <summary>
        /// 检验值
        /// </summary>
        public string InspectionValue { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum IsQualified { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }
}
