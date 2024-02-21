
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Services.Dtos.Qual;

/// <summary>
/// <para>@描述：IQC检验项目详情; 基础数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// <para><seealso cref="BaseEntityDto">点击查看享元对象</seealso></para>
/// </summary>
public record QualIqcInspectionItemDetailDto : BaseEntityDto
{
    /// <summary>
    /// IQC检验项目Id
    /// </summary>
    public long? QualIqcInspectionItemId { get; set; }

    /// <summary>
    /// 参数Id proc_parameter 的 id
    /// </summary>
    public long? ParameterId { get; set; }

    /// <summary>
    /// 项目类型;1、计量2、计数
    /// </summary>
    public IQCProjectTypeEnum? Type { get; set; }

    /// <summary>
    /// 检验器具
    /// </summary>
    public IQCUtensilTypeEnum? Utensil { get; set; }

    /// <summary>
    /// 小数位数
    /// </summary>
    public int? Scale { get; set; }

    /// <summary>
    /// 规格下限
    /// </summary>
    public decimal? LowerLimit { get; set; }

    /// <summary>
    /// 规格中心
    /// </summary>
    public decimal? Center { get; set; }

    /// <summary>
    /// 规格上限
    /// </summary>
    public decimal? UpperLimit { get; set; }

    /// <summary>
    /// 检验类型
    /// </summary>
    public IQCInspectionTypeEnum? InspectionType { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 站点ID
    /// </summary>
    public long? SiteId { get; set; }

}

/// <summary>
/// <para>@描述：IQC检验项目详情; 用于更新数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// <para><seealso cref="QualIqcInspectionItemDetailDto">点击查看享元对象</seealso></para>
/// </summary>
public record QualIqcInspectionItemDetailUpdateDto : QualIqcInspectionItemDetailDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }
}

/// <summary>
/// <para>@描述：IQC检验项目详情; 用于页面展示的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// <para><seealso cref="QualIqcInspectionItemDetailUpdateDto">点击查看享元对象</seealso></para>
/// </summary>
public record QualIqcInspectionItemDetailOutputDto : QualIqcInspectionItemDetailUpdateDto
{
    /// <summary>
    /// 参数编码
    /// </summary>
    public string? ParameterCode { get; set; }

    /// <summary>
    /// 参数名称
    /// </summary>
    public string? ParameterName {  get; set; }

    /// <summary>
    /// 参数单位
    /// </summary>
    public string? ParameterUnit {  get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedBy { get; set; }

}

/// <summary>
/// <para>@描述：IQC检验项目详情; 用于删除数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public record QualIqcInspectionItemDetailDeleteDto : QualIqcInspectionItemDetailDto
{
    /// <summary>
    /// 要删除的组
    /// </summary>
    public IEnumerable<long> Ids { get; set; }
}

/// <summary>
/// <para>@描述：IQC检验项目详情; 用于条件查询（分页）的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class QualIqcInspectionItemDetailPagedQueryDto : PagerInfo
{
}

/// <summary>
/// <para>@描述：IQC检验项目详情; 用于条件查询的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class QualIqcInspectionItemDetailQueryDto : QueryDtoAbstraction
{
    /// <summary>
    /// IQC检验项目Id
    /// </summary>
    public long? QualIqcInspectionItemId {  get; set; }
}