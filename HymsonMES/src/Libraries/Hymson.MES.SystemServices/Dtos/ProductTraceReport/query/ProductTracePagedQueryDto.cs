using Hymson.Infrastructure;
 

namespace Hymson.MES.SystemServices.Dtos.ProductTraceReport.Query;

/// <summary>
/// 追溯报表查询
/// </summary>
public class ProductTracePagedQueryDto : PagerInfo
{
    /// <summary>
    /// 条码
    /// </summary>
    public string? SFC { get; set; }

    /// <summary>
    /// true 正向，false 反向
    /// 默认正向追溯
    /// </summary>
    public bool TraceDirection { get; set; } = true;
}
