using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Plan;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（车间作业控制报告）
    /// @author Karl
    /// @date 2023-04-21 17:34:17
    /// </summary>

    [ApiController]
    [Route("api/v1/[controller]")]
    public class WorkshopJobControlReportController : ControllerBase
    {
        /// <summary>
        /// 接口（不良报告）
        /// </summary>
        private readonly ILogger<BadRecordReportController> _logger;

        private readonly IWorkshopJobControlReportService _workshopJobControlReportService;

        /// <summary>
        /// 构造函数（不良报告）
        /// </summary>
        /// <param name="badRecordReportService"></param>
        public WorkshopJobControlReportController( ILogger<BadRecordReportController> logger, IWorkshopJobControlReportService workshopJobControlReportService)
        {
            _logger = logger;
            _workshopJobControlReportService = workshopJobControlReportService;
        }


        /// <summary>
        /// 分页查询列表（车间作业控制）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<WorkshopJobControlReportViewDto>> QueryPagedWorkshopJobControlAsync([FromQuery] WorkshopJobControlReportPagedQueryDto param)
        {
            return await _workshopJobControlReportService.GetWorkshopJobControlPageListAsync(param);
        }

    }
}