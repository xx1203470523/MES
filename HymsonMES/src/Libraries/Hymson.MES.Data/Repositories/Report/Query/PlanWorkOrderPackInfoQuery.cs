using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report;

/// <summary>
/// 工单Pack追溯数据查询
/// </summary>
public class PackTraceQuery : PagerInfo
{
    /// <summary>
    /// 工单号
    /// </summary>
    public string? WorkOrderCode { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    public string? SFC { get; set; }
}

/// <summary>
/// 工单Pack数据查询
/// </summary>
public class PackTestQuery : PagerInfo
{
    /// <summary>
    /// 工单号
    /// </summary>
    public string? WorkOrderCode { get; set; }

    /// <summary>
    /// 条码
    /// </summary>
    public string? SFC { get; set; }
}
