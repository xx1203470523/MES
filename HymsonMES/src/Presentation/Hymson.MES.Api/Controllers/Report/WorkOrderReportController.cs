using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.NioPushCollection;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（工单报告）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WorkOrderReportController : ControllerBase
    {
        /// <summary>
        /// 接口（工单报告）
        /// </summary>
        private readonly ILogger<BadRecordReportController> _logger;

        private readonly IWorkOrderControlReportService _workOrderControlReportService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="workOrderControlReportService"></param>
        public WorkOrderReportController(IWorkOrderControlReportService workOrderControlReportService)
        {
            _workOrderControlReportService = workOrderControlReportService;
        }
        /// <summary>
        /// 分页查询列表（工单报告）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<WorkOrderControlReportViewDto>> QueryPagedWorkOrderControlAsync([FromQuery] WorkOrderControlReportOptimizePagedQueryDto param)
        {
            return await _workOrderControlReportService.GetWorkOrderControlPageListAsync(param);
        }

        /// 导出信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        [PermissionDescription("proc:parameter:export")]
        public async Task<NioPushCollectionExportResultDto> ExprotComUsagePageListAsync([FromQuery] WorkOrderControlReportOptimizePagedQueryDto param)
        {
            return await _workOrderControlReportService.ExprotAsync(param);
        }

    }
}