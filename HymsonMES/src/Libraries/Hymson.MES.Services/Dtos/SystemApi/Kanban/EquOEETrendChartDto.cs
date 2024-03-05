using Hymson.MES.Core.Enums.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.SystemApi.Kanban;


/// <summary>
/// 设备OEE趋势图（日/月）
/// </summary>
public class EquOEETrendChartViewDto
{
    /// <summary>
    /// 日期
    /// </summary>
    public string? EndTime { get; set; }

    /// <summary>
    /// OEE
    /// </summary>
    public decimal? OEE { get; set;}
}

public class EquOEETrendChartQueryDto
{
    /// <summary>
    /// 查询日期类型
    /// </summary>
    public DateTypeEnum? DateType { get; set; }
}