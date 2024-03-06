using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.SystemApi.Kanban;

/// <summary>
/// Pack生产数据
/// </summary>
public class PackProductionViewDto
{
    /// <summary>
    /// 产出时间
    /// </summary>
    public string? EndTime { get; set; }

    /// <summary>
    /// 计划数量
    /// </summary>
    public decimal? PlanOutputQty { get; set; }

    /// <summary>
    /// 产出数量
    /// </summary>
    public decimal? OutputQty { get; set; }

    /// <summary>
    /// 计划完成率
    /// </summary>
    public decimal? PlanCompletionRate { get; set; }
}

/// <summary>
/// Pack生产数据参数查询
/// </summary>
public class PackProductionQueryDto
{
    /// <summary>
    /// 工单编号
    /// </summary>
    public string? OrderCode { get; set; }
}