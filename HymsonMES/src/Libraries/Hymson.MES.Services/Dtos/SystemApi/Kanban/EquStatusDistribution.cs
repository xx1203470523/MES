using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.SystemApi.Kanban;

public class EquStatusDistributionViewDto
{
    /// <summary>
    /// 设备状态名称
    /// </summary>
    public string? EquipmentStatusName { get; set; }

    /// <summary>
    /// 设备状态
    /// </summary>
    public EquipmentStateEnum? EquipmentStatus { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public decimal? Qty { get; set; }
}
