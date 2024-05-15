using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（设备开机参数报表）
    /// @author huangjiayun
    /// @date 2024-05-14 10:36:01
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquStartupParameterReportController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquStartupParameterReportController> _logger;
        /// <summary>
        /// 服务接口（设备开机参数报表）
        /// </summary>
        private readonly IEquStartupParameterReportService _parameterReportService;


        /// <summary>
        /// 构造函数（设备过程参数报表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="parameterReportService"></param>
        public EquStartupParameterReportController(ILogger<EquStartupParameterReportController> logger, IEquStartupParameterReportService parameterReportService)
        {
            _logger = logger;
            _parameterReportService = parameterReportService;
        }

        /// <summary>
        /// 分页查询列表（设备开机参数报表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquStartupParameterReportDto>> QueryPagedEquipmentProcessParameterReportAsync([FromQuery] EquStartupParameterReportPagedQueryDto pagedQueryDto)
        {
            return await _parameterReportService.GetPagedListAsync(pagedQueryDto);
        }
    }
}