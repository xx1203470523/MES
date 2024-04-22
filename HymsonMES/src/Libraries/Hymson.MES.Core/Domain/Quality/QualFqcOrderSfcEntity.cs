using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（FQC检验单条码记录）   
    /// qual_fqc_order_sfc
    /// @author Jam
    /// @date 2024-03-29 04:29:24
    /// </summary>
    public class QualFqcOrderSfcEntity : BaseEntity
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
        /// 工单Id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 不合格处理方式(1-合格 2-让步 3-降级 4-维修 5-报废)
        /// </summary>
        public FQCSFCHandMethodSelectEnum? HandMethod { get; set; }

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
