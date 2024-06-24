using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 资质认证新增/更新Dto
    /// </summary>
    public record InteQualificationAuthenticationSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; } = 0;

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态 1、启用  2、禁用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 关联设置
        /// </summary>
        public List<InteQualificationAuthenticationDetailsDto>? Details { get; set; }
    }

    /// <summary>
    /// 资质认证Dto
    /// </summary>
    public record InteQualificationAuthenticationDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态 1、启用  2、禁用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }

    /// <summary>
    /// 资质认证分页Dto
    /// </summary>
    public class InteQualificationAuthenticationPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态 1、启用  2、禁用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }
    }

    /// <summary>
    /// 角色、人员资质认证Dto
    /// </summary>
    public record InteQualificationAuthenticationDetailsDto : BaseEntityDto
    {
        /// <summary>
        /// 类型 1、人员 2、角色
        /// </summary>
        public QualificationAuthenticationTypeEnum Type { get; set; }

        /// <summary>
        /// 角色、人员选择的组信息
        /// </summary>
        public IEnumerable<string>? Values { get; set; }
    }

}
