using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（FQC检验单样品检验详情）   
    /// qual_fqc_order_sample_detail
    /// @author Jam
    /// @date 2024-03-25 06:03:17
    /// </summary>
    public class QualFqcOrderSampleDetailEntity : BaseEntity
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
        /// FQC检验单样品Id
        /// </summary>
        public long FQCOrderSampleId { get; set; }

        /// <summary>
        /// FQC检验参数组明细快照Id
        /// </summary>
        public long GroupDetailSnapshootId { get; set; }

        /// <summary>
        /// 检验值
        /// </summary>
        public string InspectionValue { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum IsQualified { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        
    }
}
