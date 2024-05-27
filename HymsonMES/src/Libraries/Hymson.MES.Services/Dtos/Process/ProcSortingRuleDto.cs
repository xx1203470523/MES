/*
 *creator: Karl
 *
 *describe: 分选规则    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:24:54
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 分选规则Dto
    /// </summary>
    public record ProcSortingRuleDto : BaseEntityDto
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
        /// 规则编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 是否默认版本
        /// </summary>
        public bool? IsDefaultVersion { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

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
    /// 分选规则新增Dto
    /// </summary>
    public record ProcSortingRuleCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 规则编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 是否默认版本
        /// </summary>
        public bool? IsDefaultVersion { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 参数详情
        /// </summary>
        public IEnumerable<SortingParamDto>? SortingParamDtos { get; set; }

        /// <summary>
        /// 档次
        /// </summary>
        public IEnumerable<SortingRuleGradeDto>? SortingRuleGradeDtos { get; set; }
    }

    /// <summary>
    /// 分选规则更新Dto
    /// </summary>
    public record ProcSortingRuleModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否默认版本
        /// </summary>
        public bool? IsDefaultVersion { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 参数详情
        /// </summary>
        public IEnumerable<SortingParamDto> SortingParamDtos { get; set; }

        /// <summary>
        /// 档次
        /// </summary>
        public IEnumerable<SortingRuleGradeDto> SortingRuleGradeDtos { get; set; }
    }

    /// <summary>
    /// 分选规则分页Dto
    /// </summary>
    public class ProcSortingRulePagedQueryDto : PagerInfo
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
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 是否默认版本
        /// </summary>
        public bool? IsDefaultVersion { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;

        /// <summary>
        /// 物料id
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }


        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }
    }

    /// <summary>
    /// 参数详情
    /// </summary>
    public class SortingParamDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int? Serial { get; set; }

        /// <summary>
        /// proc_procedure 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// proc_parameter 参数Id
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 包含最小值类型;1＜ 2.≤
        /// </summary>
        public ContainingTypeEnum? MinContainingType { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 包含最大值类型;1＜ 2.≤
        /// </summary>
        public ContainingTypeEnum? MaxContainingType { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public decimal? ParameterValue { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public string Rating { get; set; }
    }

    /// <summary>
    /// 档位
    /// </summary>
    public class SortingRuleGradeDto
    {
        /// <summary>
        /// 等级
        /// </summary>
        public IEnumerable<string> Ratings { get; set; }

        /// <summary>
        /// 档位
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
