using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（iqc检验附件）   
    /// qual_iqc_order_annex
    /// @author User
    /// @date 2024-03-06 02:26:18
    /// </summary>
    public class QualIqcOrderAnnexEntity : BaseEntity
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
        /// 附件id
        /// </summary>
        public long AnnexId { get; set; }

       
    }
}
