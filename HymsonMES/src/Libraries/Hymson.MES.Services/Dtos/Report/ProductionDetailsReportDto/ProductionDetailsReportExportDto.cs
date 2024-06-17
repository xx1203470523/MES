using Confluent.Kafka;
using Hymson.Excel.Abstractions.Attributes;
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Mysqlx.Crud;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report;

[SheetDescriptionAttribute("Ng记录报表")]
public record ProductionDetailsReportExportDto : BaseExcelDto
{
    /// <summary>
    /// 产品条码
    /// </summary>
    [EpplusTableColumn(Header = "产品条码", Order = 1)]
    public string? SFC { get; set; }

    /// <summary>
    /// 设备编码
    /// </summary>
    [EpplusTableColumn(Header = "设备编码", Order = 2)]
    public string? EquipmentCode { get; set; }

    /// <summary>
    /// 设备名称
    /// </summary>
    [EpplusTableColumn(Header = "设备名称", Order = 3)]
    public string? EquipmentName { get; set; }

    /// <summary>
    /// 工序编码
    /// </summary>
    [EpplusTableColumn(Header = "工序编码", Order = 4)]
    public string? ProcedureCode { get; set; }

    /// <summary>
    /// 工序名称
    /// </summary>
    [EpplusTableColumn(Header = "工序名称", Order = 5)]
    public string? ProcedureName { get; set; }

    /// <summary>
    /// 资源编码
    /// </summary>
    [EpplusTableColumn(Header = "资源编码", Order = 6)]
    public string? ResourceCode { get; set; }

    /// <summary>
    /// 资源名称
    /// </summary>
    [EpplusTableColumn(Header = "资源名称", Order = 7)]
    public string? ResourceName { get; set; }

    /// <summary>
    /// 是否合格状态
    /// </summary>
    [EpplusTableColumn(Header = "是否合格状态", Order = 8)]
    public TrueOrFalseEnum? Passed { get; set; }

    /// <summary>
    /// 过站时间
    /// </summary>
    [EpplusTableColumn(Header = "过站时间", Order = 9)]
    public string? CreatedOn { get; set; }
}
