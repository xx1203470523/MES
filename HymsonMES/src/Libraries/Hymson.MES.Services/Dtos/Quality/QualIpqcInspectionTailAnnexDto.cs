using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Quality
{
    #region 请求参数DTO

    /// <summary>
    /// 尾检附件新增Dto
    /// </summary>
    public record QualIpqcInspectionTailAnnexSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 附件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 附件路径
        /// </summary>
        public string Path { get; set; }
    }

    /// <summary>
    /// 尾检附件分页Dto
    /// </summary>
    public class QualIpqcInspectionTailAnnexPagedQueryDto : PagerInfo { }

    #endregion

    #region 返回值DTO

    /// <summary>
    /// 尾检附件Dto
    /// </summary>
    public record QualIpqcInspectionTailAnnexDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 尾检检验单Id
        /// </summary>
        public long IpqcInspectionTailId { get; set; }

        /// <summary>
        /// 附件id
        /// </summary>
        public long AnnexId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


        /// <summary>
        /// 附件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 附件路径
        /// </summary>
        public string Path { get; set; }
    }

    #endregion
}
