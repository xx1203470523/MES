using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 单位维护新增/更新Dto
    /// </summary>
    public record InteUnitSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }


    }

    /// <summary>
    /// 单位维护Dto
    /// </summary>
    public record InteUnitDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

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


    }

    /// <summary>
    /// 单位维护分页Dto
    /// </summary>
    public class InteUnitPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 单位编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string? Name { get; set; }
    }

}
