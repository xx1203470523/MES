using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（Iqc不合格处理）   
    /// qual_iqc_order_unqualified_hand
    /// @author Czhipu
    /// @date 2024-03-06 02:27:00
    /// </summary>
    public class QualIqcOrderUnqualifiedHandEntity : BaseEntity
    {
        /// <summary>
        /// IQC检验单Id
        /// </summary>
        public long IQCOrderId { get; set; }

        /// <summary>
        /// 来源系统;1、本系统 2、OA
        /// </summary>
        public int? SourceSystem { get; set; }

        /// <summary>
        /// 不合格处理方式;1-让步；2-挑选；3-返工；4-报废；
        /// </summary>
        public HandMethodEnum? HandMethod { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string ProcessedBy { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? ProcessedOn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }


    }
}
