using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.SystemApi;

/// <summary>
/// 首页-OEE趋势图
/// </summary>
public class OEETrendChartViewDto
{
    /// <summary>
    /// 时间
    /// </summary>
    public string? EndTime { get; set; }

    /// <summary>
    /// 类型（电芯，模组，Pack）
    /// </summary>
    public string? Type { get; set;}

    /// <summary>
    /// 时间性能稼动率
    /// </summary>
    public decimal? OEE { get; set;}

}

/// <summary>
/// 首页-OEE趋势图 查询条件
/// </summary>
public class OEETrendChartQueryDto
{
    /// <summary>
    /// 工单编号
    /// </summary>
    public string? OrderCode { get; set; }
}