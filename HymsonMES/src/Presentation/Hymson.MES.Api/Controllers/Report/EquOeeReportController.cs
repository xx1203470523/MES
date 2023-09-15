using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report.EquHeartbeatReport;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 设备OEE报表控制器
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquOeeReportController : Controller
    {
        private readonly IEquOeeReportService _equOeeReportService;

        public EquOeeReportController(IEquOeeReportService equOeeReportService)
        {
            _equOeeReportService = equOeeReportService;
        }

        /// <summary>
        /// OEE 报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquOeeReportViewDto>> GetEquOeeReportPageListAsync([FromQuery] EquOeeReportPagedQueryDto param)
        {
            return await _equOeeReportService.GetEquOeeReportPageListAsync(param);
        }

        /// <summary>
        /// OEE 导出
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        [HttpGet("export")]
        public async Task<ExportResultDto> EquOeeReportExportAsync([FromQuery] EquOeeReportPagedQueryDto pageQuery)
        {
            return await _equOeeReportService.EquOeeReportExportAsync(pageQuery);
        }
    }
}