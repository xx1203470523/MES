using Hymson.MES.Core.Enums.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.SystemApi;

/// <summary>
/// 缺陷分布
/// </summary>
public class DefectDistributionViewDto
{
    /// <summary>
    /// 不合格名称
    /// </summary>
    public string UnQualifiedName { get; set; }

    /// <summary>
    /// 不良数
    /// </summary>
    public decimal UnQualifiedQty { get; set; }
}

/// <summary>
/// 缺陷分布查询条件
/// </summary>
public class DefectDistributionQueryDto
{
    /// <summary>
    /// 风冷/液冷
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// 日期类型
    /// </summary>
    public DateTypeEnum? DateType { get; set; } = DateTypeEnum.Day;

    /// <summary>
    /// Cell,Module,Pack
    /// </summary>
    public SFCTypeEnum? Type { get; set; } = SFCTypeEnum.Cell;
}