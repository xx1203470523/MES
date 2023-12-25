using Confluent.Kafka;
using Hymson.Excel.Abstractions.Attributes;
using Hymson.Infrastructure;
using Mysqlx.Crud;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report.PackTraceSfcDto;

/// <summary>
/// Pack追溯电芯码导出模板
/// </summary>
[SheetDescriptionAttribute("Pack追溯电芯码")]
public record PackTraceSfcExcelDto : BaseExcelDto
{
    /// <summary>
    /// Pack码
    /// </summary>
    [EpplusTableColumn(Header = "Pack码", Order = 1)]
    public string Pack { get; set; }

    /// <summary>
    /// 模组码
    /// </summary>
    [EpplusTableColumn(Header = "模组码", Order = 2)]
    public string Module { get; set; }

    /// <summary>
    /// 电芯码
    /// </summary>
    [EpplusTableColumn(Header = "电芯码", Order = 3)]
    public string SFC { get; set; }
}