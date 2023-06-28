using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Plan;
using Hymson.MES.Services.Services.Report.EquHeartbeatReport;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 设备状态报表控制器
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquHeartbeatReportController : ControllerBase
    {
        private readonly IEquHeartbeatReportService _equHeartbeatReportService;

        public EquHeartbeatReportController(IEquHeartbeatReportService equHeartbeatReportService)
        {
            _equHeartbeatReportService = equHeartbeatReportService;
        }
        /// <summary>
        /// 根据查询条件获取设备心跳状态报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquHeartbeatReportViewDto>> GetEquHeartbeatReportPageListAsync([FromQuery] EquHeartbeatReportPagedQueryDto param)
        {
            return await _equHeartbeatReportService.GetEquHeartbeatReportPageListAsync(param);
        }
    }
}
