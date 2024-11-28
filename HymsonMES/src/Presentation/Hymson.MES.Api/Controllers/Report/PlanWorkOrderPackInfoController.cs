using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report;

/// <summary>
/// 工单产量报表控制器
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class PlanWorkOrderPackInfoController : ControllerBase
{
    private readonly IPlanWorkOrderPackInfoService _planWorkOrderPackInfoService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public PlanWorkOrderPackInfoController(IPlanWorkOrderPackInfoService planWorkOrderPackInfoService)
    {
        _planWorkOrderPackInfoService = planWorkOrderPackInfoService;
    }

    /// <summary>
    /// Pack追溯数据查询
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    [HttpGet("packTrace/getList")]
    public async Task<PagedInfo<PackTraceOutputDto>> GetPackTraceListAsync([FromQuery]PackTraceQueryDto queryDto)
    {
        return await _planWorkOrderPackInfoService.GetTraceListAsync(queryDto);
    }

    /// <summary>
    /// Pack数据查询
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    [HttpGet("packTest/getList")]
    public async Task<PagedInfo<PackTestOutputDto>> GetPackTestListAsync([FromQuery] PackTestQueryDto queryDto)
    {
        return await _planWorkOrderPackInfoService.GetTestListAsync(queryDto);
    }

    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    [HttpGet("export")]
    public async Task<ExportResultDto> ExportExcelAsync([FromQuery] PackTraceQueryDto queryDto)
    {
        return await _planWorkOrderPackInfoService.ExportExcelAsync(queryDto);
    }
}
