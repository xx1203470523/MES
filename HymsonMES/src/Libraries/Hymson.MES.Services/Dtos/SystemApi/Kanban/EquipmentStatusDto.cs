using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.SystemApi;

/// <summary>
/// 设备运行状态
/// </summary>
public class EquipmentStatusDto
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
    /// 设备运行状态
    /// </summary>
    public EquipmentStateEnum? Status { get; set; }

    /// <summary>
    /// 实时时间
    /// </summary>
    public DateTime? RealTime { get; set; }
}
