using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report;

/// <summary>
/// Pack绑定外箱码
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class PackBindOtherReportController : ControllerBase
{
    private IPackBindOtherReportService _packBindOtherReportService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public PackBindOtherReportController(IPackBindOtherReportService packBindOtherReportService)
    {
        _packBindOtherReportService = packBindOtherReportService;
    }

    /// <summary>
    /// 分页查询数据
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("pagelist")]
    public async Task<PagedInfo<PackBindOtherReportViewDto>> GetPagedInfoAsync([FromQuery]PackBindOtherPageQueryPagedDto query)
    {
        return await _packBindOtherReportService.GetPagedInfoAsync(query);
    }

    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("export")]
    public async Task<ExportResultDto> ExportExcelAsync([FromQuery] PackBindOtherQueryDto query)
    {
        return await _packBindOtherReportService.ExportExcelAsync(query);
    }
}
