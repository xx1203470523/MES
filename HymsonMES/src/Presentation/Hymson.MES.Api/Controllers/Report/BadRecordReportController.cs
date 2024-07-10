using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（不良报告）
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
        /// <param name="logger"></param>
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

        /// <summary>
        /// 查询前十列表（不良报告）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("topTenList")]
        public async Task<List<ManuProductBadRecordReportViewDto>> QueryTopTenBadRecordAsync([FromQuery] BadRecordReportDto parm)
        {
            return await _badRecordReportService.GetTopTenBadRecordAsync(parm);
        }

        /// <summary>
        /// 分页查询列表（不良报告日志）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("logPageList")]
        public async Task<PagedInfo<ManuProductBadRecordLogReportViewDto>> GetLogPageListAsync([FromQuery] ManuProductBadRecordLogReportPagedQueryDto parm)
        {
            return await _badRecordReportService.GetLogPageListAsync(parm);
        }

        /// <summary>
        /// 查询不合格代码列表（不良报告日志）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("logPageDetailList")]
        public async Task<IEnumerable<ManuProductBadRecordLogReportResponseDto>> GetLogPageDetailListAsync([FromQuery] ManuProductBadRecordLogReportRequestDto request)
        {
            return await _badRecordReportService.GetLogPageDetailListAsync(request);
        }
    }
}