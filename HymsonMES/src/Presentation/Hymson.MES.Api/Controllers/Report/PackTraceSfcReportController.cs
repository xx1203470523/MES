using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Dtos.Report.PackTraceSfcDto;
using Hymson.MES.Services.Services.Report.PackTraceSfc;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report;

[ApiController]
[Route("api/v1/[controller]")]
public class PackTraceSfcReportController : ControllerBase
{
    private readonly IPackTraceSfcService _packTraceSfcService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public PackTraceSfcReportController(IPackTraceSfcService packTraceSfcService)
    {
        _packTraceSfcService = packTraceSfcService; 
    }

    [HttpGet]
    [Route("list")]
    public async Task<IEnumerable<PackTraceSfcViewDto>> GetListAsync([FromQuery]PackTraceSfcQueryDto queryDto)
    {
        return await _packTraceSfcService.GetListAsync(queryDto);
    }

    [HttpGet]
    [Route("page")]
    public async Task<PagedInfo<PackTraceSfcViewDto>> GetPageInfoAsync([FromQuery] PackTraceSfcPageQueryDto queryDto)
    {
        return await _packTraceSfcService.GetPageInfoAsync(queryDto);
    }

    [HttpGet]
    [Route("export")]
    public async Task<ExportResultDto> ExportExcelAsync([FromQuery]PackTraceSfcQueryDto queryDto)
    {
        return await _packTraceSfcService.ExportExcelAsyc(queryDto);
    }

}