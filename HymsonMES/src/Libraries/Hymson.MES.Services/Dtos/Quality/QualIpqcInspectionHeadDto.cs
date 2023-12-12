using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 附件上传Dto
    /// </summary>
    public record AttachmentAddDto
    {
        /// <summary>
        /// 检验单Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 附件列表
        /// </summary>
        public IEnumerable<QualIpqcInspectionHeadAnnexSaveDto> Attachments { get; set; }
    }
}
