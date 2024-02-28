using Hymson.MES.Core.Enums.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.SystemApi;

/// <summary>
/// 首页-工单基本信息
/// </summary>
public class PlanWorkOrderInfoViewDto
{
    /// <summary>
    /// 车间Id
    /// </summary>
    public long? WorkCenterId { get; set; }

    /// <summary>
    /// 车间名称
    /// </summary>
    public string? WorkCenterName { get; set; }

    /// <summary>
    /// 完成率
    /// </summary>
    public decimal? CompletionRate { get; set; }

    /// <summary>
    /// 班制类型
    /// </summary>
    public DetailClassTypeEnum? ClassType { get; set; }

    /// <summary>
    /// 工艺路线ID
    /// </summary>
    public long? ProcessRouteId { get; set; }

    /// <summary>
    /// 工艺路线名称
    /// </summary>
    public string ProcessRouteName { get; set; }

    /// <summary>
    /// 工单下达时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 工单号
    /// </summary>
    public string? OrderCode { get; set; }

    /// <summary>
    /// 工单数量
    /// </summary>
    public decimal? Qty { get; set; }

    /// <summary>
    /// 完单数量
    /// </summary>
    public decimal? Completionty { get; set; }

    /// <summary>
    /// 不合格数量
    /// </summary>
    public decimal? UnqualifiedQty { get; set; }

    /// <summary>
    /// 不良率
    /// </summary>
    public decimal? UnqualifiedRate { get; set; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public decimal ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 综合良率
    /// </summary>
    public decimal? QualifiedRate { get; set; }

    /// <summary>
    /// 综合计划达成率
    /// </summary>
    public decimal? PlanAchievementRate { get; set; }

    /// <summary>
    /// 电芯数量
    /// </summary>
    public decimal CellQty { get; set; }
}


/// <summary>
/// 查询条件
/// </summary>
public class PlanWorkOrderInfoQueryDto : QueryDtoAbstraction
{
    /// <summary>
    /// 工单编号
    /// </summary>
    public string? OrderCode { get; set; }
}
