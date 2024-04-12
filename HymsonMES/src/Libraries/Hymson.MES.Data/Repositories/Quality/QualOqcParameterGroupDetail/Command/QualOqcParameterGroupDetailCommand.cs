using Hymson.MES.Core.Enums.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Quality;

public class QualOqcParameterGroupDetailCreateCommand : CreateCommandAbstraction
{
    /// <summary>
    /// OQC检验项目Id
    /// </summary>
    public long? ParameterGroupId { get; set; }

    /// <summary>
    /// 参数Id proc_parameter 的 id
    /// </summary>
    public long? ParameterId { get; set; }

    /// <summary>
    /// 规格下限
    /// </summary>
    public decimal? LowerLimit { get; set; }

    /// <summary>
    /// 中心值
    /// </summary>
    public decimal? CenterValue { get; set; }

    /// <summary>
    /// 规格上限
    /// </summary>
    public decimal? UpperLimit { get; set; }

    /// <summary>
    /// 检验类型
    /// </summary>
    public OQCInspectionTypeEnum? InspectionType { get; set; }

    /// <summary>
    /// 参考值
    /// </summary>
    public decimal? ReferenceValue { get; set; }

    /// <summary>
    /// 录入次数
    /// </summary>
    public int? EnterNumber { get; set; }

    /// <summary>
    /// 显示顺序
    /// </summary>
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 站点ID
    /// </summary>
    public long? SiteId { get; set; }

}


public class QualOqcParameterGroupDetailUpdateCommand : UpdateCommandAbstraction
{

    /// <summary>
    /// 主键
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// OQC检验项目Id
    /// </summary>
    public long? ParameterGroupId { get; set; }

    /// <summary>
    /// 参数Id proc_parameter 的 id
    /// </summary>
    public long? ParameterId { get; set; }

    /// <summary>
    /// 规格下限
    /// </summary>
    public decimal? LowerLimit { get; set; }

    /// <summary>
    /// 规格中心
    /// </summary>
    public decimal? CenterValue { get; set; }

    /// <summary>
    /// 规格上限
    /// </summary>
    public decimal? UpperLimit { get; set; }

    /// <summary>
    /// 检验类型
    /// </summary>
    public string? InspectionType { get; set; }

    /// <summary>
    /// 参考值
    /// </summary>
    public string? ReferenceValue { get; set; }

    /// <summary>
    /// 录入次数
    /// </summary>
    public int? EnterNumber { get; set; }

    /// <summary>
    /// 显示顺序
    /// </summary>
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 站点ID
    /// </summary>
    public long? SiteId { get; set; }

}
