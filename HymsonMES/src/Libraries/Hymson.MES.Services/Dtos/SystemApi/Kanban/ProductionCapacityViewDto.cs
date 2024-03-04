using Hymson.MES.Core.Enums.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.SystemApi;

public class ProductionCapacityViewDto
{
    /// <summary>
    /// 时间
    /// </summary>
    public string? EndTime { get; set; }

    /// <summary>
    /// 目标产能
    /// </summary>
    public decimal? PlanQty { get; set; }

    /// <summary>
    /// 产出数量
    /// </summary>
    public decimal? OutputQty { get; set; }

    /// <summary>
    /// 实际完成率
    /// </summary>
    public decimal? CompletionRate { get; set; }
}


public class ProductionCapacityQueryDto
{
    /// <summary>
    /// 风冷/液冷（分别对应不同产品）
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// 条码类型
    /// </summary>
    public SFCTypeEnum? Type { get; set; }
}
