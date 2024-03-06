using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.SystemApi;

/// <summary>
/// 一次合格率
/// </summary>
public class OneQualifiedViewDto
{
    /// <summary>
    /// 电芯一次合格率
    /// </summary>
    public decimal? CellOneQualifiedRate { get; set; }

    /// <summary>
    /// 模组一次合格率
    /// </summary>
    public decimal? ModuleOneQualifiedRate { get; set; }

    /// <summary>
    /// Pack一次合格率
    /// </summary>
    public decimal? PackOneQualifiedRate { get; set; }
}

public class OneQualifiedQueryDto
{
    /// <summary>
    /// 产品名称（风冷/液冷）
    /// </summary>
    public string? ProductName { get; set; }
}