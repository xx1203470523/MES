using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Dtos.ProductTraceReport.Query;

/// <summary>
/// 产品参数查询
/// </summary>
public class ManuProductPrameterPagedQueryDto : PagerInfo
{
    /// <summary>
    /// 条码
    /// </summary>
    public string? SFC { get; set; }
    /// <summary>
    /// 参数类型
    /// </summary>
    public ParameterTypeEnum? ParameterType { get; set; }
    /// <summary>
    ///采集开始时间
    ///CreatedOn
    /// </summary>
    public DateTime? StartTime { get; set; }
    /// <summary>
    ///采集结束时间
    ///CreatedOn
    /// </summary>
    public DateTime? EndTime { get; set; }
    /// <summary>
    ///设备上报开始时间
    ///CreatedOn
    /// </summary>
    public DateTime? LocalTimeStartTime { get; set; }
    /// <summary>
    ///设备上报结束时间
    ///CreatedOn
    /// </summary>
    public DateTime? LocalTimeEndTime { get; set; }
}