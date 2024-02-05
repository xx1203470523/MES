using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（巡检附件）   
    /// qual_ipqc_inspection_patrol_annex
    /// @author xiaofei
    /// @date 2023-08-24 10:51:33
    /// </summary>
    public class QualIpqcInspectionPatrolAnnexEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 巡检检验单Id
        /// </summary>
        public long IpqcInspectionPatrolId { get; set; }

        /// <summary>
        /// 附件id
        /// </summary>
        public long AnnexId { get; set; }


    }
}
