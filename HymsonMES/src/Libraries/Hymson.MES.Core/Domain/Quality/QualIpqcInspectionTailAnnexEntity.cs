using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（尾检附件）   
    /// qual_ipqc_inspection_tail_annex
    /// @author xiaofei
    /// @date 2023-08-24 10:52:25
    /// </summary>
    public class QualIpqcInspectionTailAnnexEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 尾检检验单Id
        /// </summary>
        public long IpqcInspectionTailId { get; set; }

        /// <summary>
        /// 附件id
        /// </summary>
        public long AnnexId { get; set; }


    }
}
