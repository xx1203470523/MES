using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Report;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class NgRecordReportController : ControllerBase
{

    private readonly INgRecordService _ngRecordService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public NgRecordReportController(INgRecordService ngRecordService)
    {
        _ngRecordService = ngRecordService;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IEnumerable<NgRecordReportDto>> GetListAsync([FromQuery] NgRecordReportQueryDto queryDto)
    {
        return await _ngRecordService.GetListAsync(queryDto);
    }

    [HttpGet]
    [Route("pageinfo")]
    public async Task<PagedInfo<NgRecordReportDto>> GetPageInfoAsync([FromQuery] NgRecordReportPageQueryDto pageQueryDto)
    {
        return await _ngRecordService.GetPageInfoAsync(pageQueryDto);
    }

    [HttpGet]
    [Route("export")]
    public async Task<ExportResultDto> ExportExcelAsync([FromQuery] NgRecordReportQueryDto queryDto)
    {
        return await _ngRecordService.ExportExcelAsync(queryDto);
    }
}
