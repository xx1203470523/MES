using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report;

/// <summary>
/// 工单Pack数据查询接口
/// </summary>
public interface IPlanWorkOrderPackInfoService
{
    /// <summary>
    /// Pack数据追溯查询
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public Task<PagedInfo<PackTraceOutputDto>> GetTraceListAsync(PackTraceQueryDto queryDto);

    /// <summary>
    /// Pack数据查询
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public Task<PagedInfo<PackTestOutputDto>> GetTestListAsync(PackTestQueryDto queryDto);

    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public Task<ExportResultDto> ExportExcelAsync(PackTraceQueryDto queryDto);
}
