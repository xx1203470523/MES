using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（FQC不合格处理结果）   
    /// qual_fqc_order_unqualified_handle
    /// @author User
    /// @date 2024-03-27 11:42:30
    /// </summary>
    public class QualFqcOrderUnqualifiedHandleEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// FQC检验单Id
        /// </summary>
        public long FQCOrderId { get; set; }

        /// <summary>
        /// 来源系统(1-MES 2-OA)
        /// </summary>
        public int? SourceSystem { get; set; }

        /// <summary>
        /// 不合格处理方式（1-让步 2-挑选 3-维修 4-报废）
        /// </summary>
        public HandMethodEnum? HandMethod { get; set; }

        /// <summary>
        /// 维修工艺路线Id
        /// </summary>
        public long? ProcessRouteId { get; set; }

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
