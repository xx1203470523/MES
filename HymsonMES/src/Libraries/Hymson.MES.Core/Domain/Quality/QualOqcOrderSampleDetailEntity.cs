using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（OQC检验单检验样本明细）   
    /// qual_oqc_order_sample_detail
    /// @author xiaofei
    /// @date 2024-03-04 10:58:47
    /// </summary>
    public class QualOqcOrderSampleDetailEntity : BaseEntity
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
        /// OQC检验单样品Id
        /// </summary>
        public long OQCOrderSampleId { get; set; }

        /// <summary>
        /// OQC检验参数组明细快照Id
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
        public string Remark { get; set; }
    }
}
