using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Plan;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 工单产量报表控制器
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WorkOrderProductionReportController : ControllerBase
    {
        private readonly IPlanWorkOrderService _planWorkOrderService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="planWorkOrderService"></param>
        public WorkOrderProductionReportController(IPlanWorkOrderService planWorkOrderService)
        {
            _planWorkOrderService = planWorkOrderService;
        }
        /// <summary>
        /// 分页查询列表（车间作业控制）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<PlanWorkOrderProductionReportViewDto>> QueryPagedWorkOrderProductionAsync([FromQuery] PlanWorkOrderProductionReportPagedQueryDto param)
        {
            return await _planWorkOrderService.GetPlanWorkOrderProductionReportPageListAsync(param);
        }
    }
}
