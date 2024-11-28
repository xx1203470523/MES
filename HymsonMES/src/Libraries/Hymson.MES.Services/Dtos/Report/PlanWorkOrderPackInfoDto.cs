using Hymson.Excel.Abstractions.Attributes;
using Hymson.Infrastructure;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report;

/// <summary>
/// 工单Pack数据追溯查询Dto
/// </summary>
public class PackTraceQueryDto : PagerInfo
{
    /// <summary>
    /// 工单号
    /// </summary>
    public string? WorkOrderCode { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    public string? SFC { get; set; }
}

/// <summary>
/// 工单Pack数据追溯Dto
/// </summary>
public record PackTraceOutputDto : BaseEntityDto
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
public class PackTestQueryDto : PagerInfo
{
    /// <summary>
    /// 工单号
    /// </summary>
    public string? WorkOrderCode { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    public string? SFC { get; set; }
}

/// <summary>
/// 工单Pack测试数据查询Dto
/// </summary>
public record PackTestOutputDto : BaseEntityDto
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






/// <summary>
/// 工单Pack数据追溯ExcelDto
/// </summary>
[SheetDescription("追溯数据")]
public record PackTraceExcelDto : BaseExcelDto
{
    /// <summary>
    /// Pack码
    /// </summary>
    [EpplusTableColumn(Header = "柜体编码", Order = 1)]
    public string? Column1 { get; set; }

    /// <summary>
    /// Pack码
    /// </summary>
    [EpplusTableColumn(Header = "簇编码", Order = 1)]
    public string? Column2 { get; set; }

    /// <summary>
    /// Pack码
    /// </summary>
    [EpplusTableColumn(Header = "Pack编码", Order = 1)]
    public string? Pack { get; set; }

    /// <summary>
    /// 模组码
    /// </summary>
    [EpplusTableColumn(Header = "模组编码", Order = 2)]
    public string? Module { get; set; }

    /// <summary>
    /// 模组电压
    /// </summary>
    [EpplusTableColumn(Header = "模组电压", Order = 3)]
    public string? ModuleVoltage { get; set; }

    /// <summary>
    /// 模组电阻
    /// </summary>
    [EpplusTableColumn(Header = "模组电阻", Order = 4)]
    public string? ModuleInternalResistance { get; set; }

    /// <summary>
    /// 电芯码
    /// </summary>
    [EpplusTableColumn(Header = "电芯编码", Order = 5)]
    public string? Cell { get; set; }

    /// <summary>
    /// 电芯电压
    /// </summary>
    [EpplusTableColumn(Header = "电芯电压", Order = 6)]
    public string? CellVoltage { get; set; }

    /// <summary>
    /// 电芯电阻
    /// </summary>
    [EpplusTableColumn(Header = "电芯电阻", Order = 7)]
    public string? CellInternalResistance { get; set; }
}



/// <summary>
/// 工单Pack测试数据ExcelDto
/// </summary>
[SheetDescription("Pack测试数据")]
public record PackTestExcelDto : BaseExcelDto
{
    /// <summary>
    /// 条码
    /// </summary>
    [EpplusTableColumn(Header = "Pack编码", Order = 1)]
    public string? SFC { get; set; }

    [EpplusTableColumn(Header = "总电压", Order = 2)]
    public string? Column1 { get; set; }

    [EpplusTableColumn(Header = "总内阻", Order = 3)]
    public string? Column2 { get; set; }

    [EpplusTableColumn(Header = "正极绝缘≥500MΩ", Order = 4)]
    public string? Column3 { get; set; }

    [EpplusTableColumn(Header = "负极绝缘≥500MΩ", Order = 5)]
    public string? Column4 { get; set; }

    [EpplusTableColumn(Header = "正极耐压≤1mA", Order = 6)]
    public string? Column5 { get; set; }

    [EpplusTableColumn(Header = "Pac负极耐压≤1mAk码", Order = 7)]
    public string? Column6 { get; set; }

    [EpplusTableColumn(Header = "BAMU版本号", Order = 8)]
    public string? Column7 { get; set; }

    [EpplusTableColumn(Header = "采集最高电压", Order = 9)]
    public string? Column8 { get; set; }

    [EpplusTableColumn(Header = "采集最低电压", Order = 10)]
    public string? Column9 { get; set; }

    [EpplusTableColumn(Header = "采集最高温度", Order = 11)]
    public string? Column10 { get; set; }

    [EpplusTableColumn(Header = "采集最低温度", Order = 12)]
    public string? Column11 { get; set; }

    [EpplusTableColumn(Header = "压差", Order = 13)]
    public string? Column12 { get; set; }

    [EpplusTableColumn(Header = "温差", Order = 14)]
    public string? Column13 { get; set; }

    [EpplusTableColumn(Header = "气压泄漏量（Pa）-50≤气压泄漏量≤50Pa", Order = 15)]
    public string? Column14 { get; set; }

    [EpplusTableColumn(Header = "充电末端电池单体最大压差", Order = 16)]
    public string? Column15 { get; set; }

    [EpplusTableColumn(Header = "放电末端电池单体最大压差", Order = 17)]
    public string? Column16 { get; set; }

    [EpplusTableColumn(Header = "充电末端电池单体最大温差", Order = 18)]
    public string? Column17 { get; set; }

    [EpplusTableColumn(Header = "放电末端电池单体最大温差", Order = 19)]
    public string? Column18 { get; set; }

}

[SheetDescription("Sheet3")]
public record Null3ExcelDto : BaseExcelDto
{
    /// <summary>
    /// 条码
    /// </summary>
    [EpplusTableColumn(Header = "", Order = 1)]
    public string? SFC { get; set; }
}

[SheetDescription("Sheet4")]
public record Null4ExcelDto : BaseExcelDto
{
    /// <summary>
    /// 条码
    /// </summary>
    [EpplusTableColumn(Header = "", Order = 1)]
    public string? SFC { get; set; }
}

[SheetDescription("Sheet5")]
public record Null5ExcelDto : BaseExcelDto
{
    /// <summary>
    /// 条码
    /// </summary>
    [EpplusTableColumn(Header = "", Order = 1)]
    public string? SFC { get; set; }
}

[SheetDescription("Sheet6")]
public record Null6ExcelDto : BaseExcelDto
{
    /// <summary>
    /// 条码
    /// </summary>
    [EpplusTableColumn(Header = "", Order = 1)]
    public string? SFC { get; set; }
}