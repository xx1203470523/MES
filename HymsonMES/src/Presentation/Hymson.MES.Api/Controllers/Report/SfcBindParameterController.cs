using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report.SfcBindParameter;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report;

/// <summary>
/// 条码绑定关系扭矩参数
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class SfcBindParameterController : ControllerBase
{
    private readonly ISfcBindParameterService _sfcBindParameterService;

    public SfcBindParameterController(ISfcBindParameterService sfcBindParameterService)
    {
        _sfcBindParameterService = sfcBindParameterService;
    }


    /// <summary>
    /// 报表查询
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    [HttpGet("getList")]
    public async Task<PagedInfo<SfcBindParameterReportPagedDataDto>> GetPagedDataAsync([FromQuery]SfcBindParameterReportPagedQueryDto queryDto)
    {
        return await _sfcBindParameterService.GetPagedDataAsync(queryDto);
    }

    /// <summary>
    /// 报表导出
    /// </summary>
    /// <returns></returns>
    [HttpGet("export")]
    public async Task<ExportResultDto> ReportExportAsync([FromQuery] SfcBindParameterReportPagedQueryDto queryDto)
    {
        return await _sfcBindParameterService.ReportExportAsync(queryDto);
    }

    /// <summary>
    /// 条码绑定关系参数查询
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    [HttpGet("getList2")]
    public async Task<PagedInfo<SfcBindParameterReport2PagedDataDto>> GetPagedData2Async([FromQuery] SfcBindParameterReport2PagedQueryDto queryDto)
    {
        return await _sfcBindParameterService.GetPagedData2Async(queryDto);
    }

    /// <summary>
    /// 报表导出
    /// </summary>
    /// <returns></returns>
    [HttpGet("export2")]
    public async Task<ExportResultDto> ReportExport2Async([FromQuery] SfcBindParameterReport2PagedQueryDto queryDto)
    {
        return await _sfcBindParameterService.ReportExport2Async(queryDto);
    }
}
