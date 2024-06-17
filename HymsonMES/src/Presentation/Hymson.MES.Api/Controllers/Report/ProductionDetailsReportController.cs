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
public class ProductionDetailsReportController : ControllerBase
{

    private readonly IProductionDetailsReportService _productionDetailsReportService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public ProductionDetailsReportController(IProductionDetailsReportService productionDetailsReportService)
    {
        _productionDetailsReportService = productionDetailsReportService;
    }

    [HttpGet]
    [Route("list")]
    public async Task<IEnumerable<ProductionDetailsReportDto>> GetListAsync([FromQuery] ProductionDetailsReportQueryDto queryDto)
    {
        return await _productionDetailsReportService.GetListAsync(queryDto);
    }

    [HttpGet]
    [Route("pageinfo")]
    public async Task<PagedInfo<ProductionDetailsReportDto>> GetPageInfoAsync([FromQuery] ProductionDetailsReportPageQueryDto pageQueryDto)
    {
        return await _productionDetailsReportService.GetPageInfoAsync(pageQueryDto);
    }

    [HttpGet]
    [Route("export")]
    public async Task<ExportResultDto> ExportExcelAsync([FromQuery] ProductionDetailsReportQueryDto queryDto)
    {
        return await _productionDetailsReportService.ExportExcelAsync(queryDto);
    }
}
