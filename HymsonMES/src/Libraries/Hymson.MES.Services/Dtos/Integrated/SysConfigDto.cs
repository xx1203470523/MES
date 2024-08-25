using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 系统配置新增/更新Dto
    /// </summary>
    public record SysConfigSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 配置类型
        /// </summary>
        public SysConfigEnum? Type { get; set; }

        /// <summary>
        /// 配置编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 配置value
        /// </summary>
        public string Value { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }    
    }

    /// <summary>
    /// 系统配置Dto
    /// </summary>
    public record SysConfigDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 配置类型
        /// </summary>
        public SysConfigEnum Type { get; set; }

        /// <summary>
        /// 配置编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 配置value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 系统配置分页Dto
    /// </summary>
    public class SysConfigPagedQueryDto : PagerInfo 
    {
        /// <summary>
        /// 配置类型
        /// </summary>
        public SysConfigEnum ?Type { get; set; }

        /// <summary>
        /// 配置编码
        /// </summary>
        public string ?Code { get; set; }
    }

}
