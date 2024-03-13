using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 附件保存
    /// </summary>
    public record QualOqcOrderSaveAttachmentDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long OQCOrderId { get; set; }

        /// <summary>
        /// IQC检验单（附件）
        /// </summary>
        public IEnumerable<InteAttachmentBaseDto> Attachments { get; set; }

    }
}
