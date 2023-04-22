using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Plan;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（不良报告）
    /// @author Karl
    /// @date 2023-04-21 17:34:17
    /// </summary>

    [ApiController]
    [Route("api/v1/[controller]")]
    public class BadRecordReportController : ControllerBase
    {
        /// <summary>
        /// 接口（不良报告）
        /// </summary>
        private readonly IBadRecordReportService _badRecordReportService;
        private readonly ILogger<BadRecordReportController> _logger;

        /// <summary>
        /// 构造函数（不良报告）
        /// </summary>
        /// <param name="badRecordReportService"></param>
        public BadRecordReportController(IBadRecordReportService badRecordReportService, ILogger<BadRecordReportController> logger)
        {
            _badRecordReportService = badRecordReportService;
            _logger = logger;
        }


        /// <summary>
        /// 分页查询列表（不良报告）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuProductBadRecordReportViewDto>> QueryPagedBadRecordReportAsync([FromQuery] BadRecordReportDto parm)
        {
            return await _badRecordReportService.GetPageListAsync(parm);
        }

    }
}