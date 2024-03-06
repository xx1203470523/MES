using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（OQC检验单样本明细附件）   
    /// qual_oqc_order_sample_detail_annex
    /// @author xiaofei
    /// @date 2024-03-04 11:00:11
    /// </summary>
    public class QualOqcOrderSampleDetailAnnexEntity : BaseEntity
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
        /// 样本明细Id
        /// </summary>
        public long SampleDetailId { get; set; }

        /// <summary>
        /// 附件Id
        /// </summary>
        public long AnnexId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
