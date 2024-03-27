using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（FQC检验单样本明细附件）   
    /// qual_fqc_order_sample_detail_attachment
    /// @author Jam
    /// @date 2024-03-25 06:08:56
    /// </summary>
    public class QualFqcOrderSampleDetailAttachmentEntity : BaseEntity
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
        /// 样本明细Id
        /// </summary>
        public long SampleDetailId { get; set; }

        /// <summary>
        /// 附件Id
        /// </summary>
        public long AttachmentId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        
    }
}
