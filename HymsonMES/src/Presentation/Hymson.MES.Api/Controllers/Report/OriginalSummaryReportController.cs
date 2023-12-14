using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（原建汇总）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OriginalSummaryReportController : ControllerBase
    {
        /// <summary>
        /// 接口（原建汇总）
        /// </summary>
        private readonly IOriginalSummaryReportService _originalSummaryReportService;
        private readonly ILogger<OriginalSummaryReportController> _logger;

        /// <summary>
        /// 构造函数（原建汇总）
        /// </summary>
        /// <param name="originalSummaryReportService"></param>
        /// <param name="logger"></param>
        public OriginalSummaryReportController(IOriginalSummaryReportService originalSummaryReportService, ILogger<OriginalSummaryReportController> logger)
        {
            _originalSummaryReportService = originalSummaryReportService;
            _logger = logger;
        }

        /// <summary>
        /// 查询列表（原建汇总）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<List<OriginalSummaryReportDto>> GetOriginalSummaryAsync([FromQuery]OriginalSummaryQueryDto queryDto)
        {
            return await _originalSummaryReportService.GetOriginalSummaryAsync(queryDto);
        }
    }
}