using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（首检附件）   
    /// qual_ipqc_inspection_head_annex
    /// @author xiaofei
    /// @date 2023-08-21 06:13:46
    /// </summary>
    public class QualIpqcInspectionHeadAnnexEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 首检检验单Id
        /// </summary>
        public long IpqcInspectionHeadId { get; set; }

        /// <summary>
        /// 附件id
        /// </summary>
        public long AnnexId { get; set; }


    }
}
