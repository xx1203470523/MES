using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（首检附件）   
    /// qual_iqc_order_sample_detail_annex
    /// @author Czhipu
    /// @date 2024-03-06 02:26:45
    /// </summary>
    public class QualIqcOrderSampleDetailAnnexEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// IQC检验单Id
        /// </summary>
        public long IQCOrderId { get; set; }

        /// <summary>
        /// Iqc样本Id
        /// </summary>
        public long IQCOrderSampleDetailId { get; set; }

       /// <summary>
        /// 附件id
        /// </summary>
        public long AnnexId { get; set; }

       
    }
}
