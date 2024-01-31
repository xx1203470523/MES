using Hymson.Infrastructure;
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

    [HttpGet]
    [Route("pagelist")]
    public async Task<PagedInfo<PackBindOtherReportViewDto>> GetPagedInfoAsync([FromQuery]PackBindOtherPageQueryPagedDto query)
    {
        return await _packBindOtherReportService.GetPagedInfoAsync(query);
    }
}
