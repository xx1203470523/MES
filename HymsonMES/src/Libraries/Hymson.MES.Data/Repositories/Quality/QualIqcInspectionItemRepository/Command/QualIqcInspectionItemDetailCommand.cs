
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Qual;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：创建指令</para>
/// <para>@描述：IQC检验项目详情;标准创建对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public class QualIqcInspectionItemDetailCreateCommand : CreateCommandAbstraction
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
/// <para>@层级：仓储层</para>
/// <para>@类型：更新指令</para>
/// <para>@描述：IQC检验项目详情;标准更新对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public class QualIqcInspectionItemDetailUpdateCommand : UpdateCommandAbstraction
{

    /// <summary>
    /// 主键
    /// </summary>
    public long? Id { get; set; }

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
    public string? Utensil { get; set; }

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
    public string? InspectionType { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 站点ID
    /// </summary>
    public long? SiteId { get; set; }

}