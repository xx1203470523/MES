using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Equipment;

/// <summary>
/// 设备报警持续时间
/// </summary>
public class EquAlarmDurationTimeDto
{
    /// <summary>
    /// 设备Id
    /// </summary>
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备报警停机持续时间（s）
    /// </summary>
    public decimal? DurationTime { get; set; }
}

/// <summary>
/// 设备报警持续时间计算类
/// </summary>
public class EquAlarmComputedDto
{
    /// <summary>
    /// 设备ID
    /// </summary>
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备报警持续时间
    /// </summary>
    public decimal? DurationTime { get; set; }

    /// <summary>
    /// 触发状态
    /// </summary>
    public EquipmentAlarmStatusEnum? Status { get; set; }

    /// <summary>
    /// 报警开始时间
    /// </summary>
    public DateTime? BeginTime { get; set; }

    /// <summary>
    /// 报警恢复时间
    /// </summary>
    public DateTime? EndTime { get; set; }
}

public class EquAlarmDurationTimeQueryDto
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? BeginTime { get; set; }

    /// <summary>
    /// 截至时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 设备Id
    /// </summary>
    public long? EquipmentId { get; set; }

    /// <summary>
    /// 设备Id
    /// </summary>
    public IEnumerable<long>? EquipmentIds { get; set; }
}