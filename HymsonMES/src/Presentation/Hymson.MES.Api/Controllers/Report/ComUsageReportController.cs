using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Plan;
using Hymson.MES.Services.Services.Report;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（组件使用报告）
    /// @author Karl
    /// @date 2023-04-27 14:55:17
    /// </summary>

    [ApiController]
    [Route("api/v1/[controller]")]
    public class ComUsageReportController : ControllerBase
    {
        /// <summary>
        /// 接口（组件使用报告）
        /// </summary>
        private readonly ILogger<BadRecordReportController> _logger;

        private readonly IComUsageReportService _comUsageReportService;

        /// <summary>
        /// 构造函数（组件使用报告）
        /// </summary>
        /// <param name="comUsageReportService"></param>
        /// <param name="logger"></param>
        public ComUsageReportController( ILogger<BadRecordReportController> logger, IComUsageReportService comUsageReportService)
        {
            _logger = logger;
            _comUsageReportService = comUsageReportService;
        }


        /// <summary>
        /// 分页查询列表（组件使用报告）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ComUsageReportViewDto>> QueryPagedComUsageAsync([FromQuery] ComUsageReportPagedQueryDto param)
        {
            return await _comUsageReportService.GetComUsagePageListAsync(param);
        }

        /// <summary>
        /// 导出组件使用报告
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        [PermissionDescription("report:comUsageReport:export")]
        public async Task<ComUsageExportResultDto> ExprotComUsagePageListAsync([FromQuery] ComUsageReportPagedQueryDto param)
        {
            return await _comUsageReportService.ExprotComUsagePageListAsync(param);
        }
    }
}