using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（OQC不合格处理结果）   
    /// qual_oqc_order_unqualified_handle
    /// @author xiaofei
    /// @date 2024-03-04 11:00:37
    /// </summary>
    public class QualOqcOrderUnqualifiedHandleEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// OQC检验单Id
        /// </summary>
        public long OQCOrderId { get; set; }

        /// <summary>
        /// 来源系统(1-MES 2-OA)
        /// </summary>
        public SourceSystemEnum SourceSystem { get; set; }

        /// <summary>
        /// 不合格处理方式（1-让步 2-挑选 3-返工 4-报废）
        /// </summary>
        public OQCHandMethodEnum? HandMethod { get; set; }

        /// <summary>
        /// 不合格处理人
        /// </summary>
        public string ProcessedBy { get; set; }

        /// <summary>
        /// 不合格处理时间
        /// </summary>
        public DateTime? ProcessedOn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
