using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（FQC检验单附件）   
    /// qual_fqc_order_attachment
    /// @author Jam
    /// @date 2024-03-26 06:36:41
    /// </summary>
    public class QualFqcOrderAttachmentEntity : BaseEntity
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
        /// 附件Id
        /// </summary>
        public long AttachmentId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        
    }
}
