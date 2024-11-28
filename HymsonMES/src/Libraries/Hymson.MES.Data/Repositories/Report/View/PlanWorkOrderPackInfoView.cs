using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report;

/// <summary>
/// 工单Pack数据追溯Dto
/// </summary>
public class PackTraceView : BaseEntity
{
    /// <summary>
    /// Pack码
    /// </summary>
    public string? Pack { get; set; }

    /// <summary>
    /// 模组码
    /// </summary>
    public string? Module { get; set; }

    /// <summary>
    /// 模组电压
    /// </summary>
    public string? ModuleVoltage { get; set; }

    /// <summary>
    /// 模组电阻
    /// </summary>
    public string? ModuleInternalResistance { get; set; }

    /// <summary>
    /// 电芯码
    /// </summary>
    public string? Cell { get; set; }

    /// <summary>
    /// 电芯电压
    /// </summary>
    public string? CellVoltage { get; set; }

    /// <summary>
    /// 电芯电阻
    /// </summary>
    public string? CellInternalResistance { get; set; }
}

/// <summary>
/// 工单Pack测试数据查询Dto
/// </summary>
public class PackTestView : BaseEntity
{
    /// <summary>
    /// 条码
    /// </summary>
    public string? SFC { get; set; }

    public string? Column1 { get; set; }

    public string? Column2 { get; set; }

    public string? Column3 { get; set; }

    public string? Column4 { get; set; }

    public string? Column5 { get; set; }

    public string? Column6 { get; set; }

    public string? Column7 { get; set; }

    public string? Column8 { get; set; }

    public string? Column9 { get; set; }

    public string? Column10 { get; set; }

    public string? Column11 { get; set; }

    public string? Column12 { get; set; }

    public string? Column13 { get; set; }

    public string? Column14 { get; set; }

    public string? Column15 { get; set; }

    public string? Column16 { get; set; }

    public string? Column17 { get; set; }

    public string? Column18 { get; set; }

}