using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.SystemApi;

/// <summary>
/// 工序日产出
/// </summary>
public class ProcedureDayOutputViewDto
{
    /// <summary>
    /// 工序Id
    /// </summary>
    public long? ProcedureId { get; set; }

    /// <summary>
    /// 工序名称
    /// </summary>
    public string? ProcedureName { get; set; }

    /// <summary>
    /// 工序计划
    /// </summary>
    public decimal? PlanQty { get; set; }

    /// <summary>
    /// 实际产出
    /// </summary>
    public decimal? OutputQty { get; set; }

    /// <summary>
    /// 计划完成率
    /// </summary>
    public decimal? PlanCompleteRate { get; set; }
}

/// <summary>
/// 工序日产出查询
/// </summary>
public class ProcedureDayOutputQueryDto
{
    /// <summary>
    /// 风冷/液冷（分别对应不同产品）
    /// </summary>
    public string? ProductName { get; set; }
}