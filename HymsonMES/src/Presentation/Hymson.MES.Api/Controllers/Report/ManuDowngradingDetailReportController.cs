using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（降级品明细报表）
    /// @author huangjiayun
    /// @date 2023-09-11 02:27:41
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuDowngradingDetailReportController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuDowngradingDetailReportController> _logger;
        /// <summary>
        /// 服务接口（降级品明细报表）
        /// </summary>
        private readonly IManuDowngradingDetailReportService _manuDowngradingDetailReportService;


        /// <summary>
        /// 构造函数（降级品明细报表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuDowngradingDetailReportService"></param>
        public ManuDowngradingDetailReportController(ILogger<ManuDowngradingDetailReportController> logger, IManuDowngradingDetailReportService manuDowngradingDetailReportService)
        {
            _logger = logger;
            _manuDowngradingDetailReportService = manuDowngradingDetailReportService;
        }

        /// <summary>
        /// 分页查询列表（降级品明细报表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuDowngradingDetailReportDto>> QueryPagedManuDowngradingDetailReportAsync([FromQuery] ManuDowngradingDetailReportPagedQueryDto pagedQueryDto)
        {
            return await _manuDowngradingDetailReportService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 导出查询列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        public async Task<WorkOrderControlExportResultDto> ExprotListAsync([FromQuery] ManuDowngradingDetailReportPagedQueryDto param)
        {
            return await _manuDowngradingDetailReportService.ExprotListAsync(param);
        }

    }
}