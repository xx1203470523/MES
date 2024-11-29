using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.NioPushCollection;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（工单步骤）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WorkOrderStepController : ControllerBase
    {
        /// <summary>
        /// 接口（工单步骤）
        /// </summary>
        private readonly ILogger<BadRecordReportController> _logger;

        private readonly IWorkOrderStepControlService _workOrderStepControlService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="workOrderStepControlService"></param>
        public WorkOrderStepController(IWorkOrderStepControlService workOrderStepControlService)
        {
            _workOrderStepControlService = workOrderStepControlService;
        }
        /// <summary>
        /// 分页查询列表（工单步骤）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<WorkOrderStepControlViewDto>> QueryPagedWorkOrderControlAsync([FromQuery] WorkOrderStepControlOptimizePagedQueryDto param)
        {
            return await _workOrderStepControlService.GetWorkOrderStepControlPageListAsync(param);
        }

        /// 导出信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        [PermissionDescription("proc:workOrderStepReport:export")]
        public async Task<NioPushCollectionExportResultDto> ExprotComUsagePageListAsync([FromQuery] WorkOrderStepControlOptimizePagedQueryDto param)
        {
            return await _workOrderStepControlService.ExprotAsync(param);
        }

    }
}