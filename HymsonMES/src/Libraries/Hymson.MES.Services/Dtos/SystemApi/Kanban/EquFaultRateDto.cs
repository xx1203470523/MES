using Hymson.MES.Core.Enums.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.SystemApi.Kanban;

/// <summary>
/// 设备故障率
/// </summary>
public class EquFaultRateViewDto
{
    /// <summary>
    /// 设备编码
    /// </summary>
    public string? EquipmentCode { get; set; }

    /// <summary>
    /// 设备名称
    /// </summary>
    public string? EquipmentName { get; set; }

    /// <summary>
    /// 故障率
    /// </summary>
    public decimal? FaultRate { get; set; }
}

/// <summary>
/// 设备故障率查询参数
/// </summary>
public class EquFaultRateQueryDto
{
    /// <summary>
    /// 日期类型（0=日，2=月）
    /// </summary>
    public DateTypeEnum? DateType  { get; set; }
}