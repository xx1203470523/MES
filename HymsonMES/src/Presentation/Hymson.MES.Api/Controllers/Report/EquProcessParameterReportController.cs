using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（设备过程参数报表）
    /// @author huangjiayun
    /// @date 2023-09-26 10:36:01
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquProcessParameterReportController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquProcessParameterReportController> _logger;
        /// <summary>
        /// 服务接口（设备过程参数报表）
        /// </summary>
        private readonly IEquProcessParameterReportService _equipmentProcessParameterReportService;


        /// <summary>
        /// 构造函数（设备过程参数报表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equipmentProcessParameterReportService"></param>
        public EquProcessParameterReportController(ILogger<EquProcessParameterReportController> logger, IEquProcessParameterReportService equipmentProcessParameterReportService)
        {
            _logger = logger;
            _equipmentProcessParameterReportService = equipmentProcessParameterReportService;
        }

        /// <summary>
        /// 分页查询列表（设备过程参数报表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquProcessParameterReportDto>> QueryPagedEquipmentProcessParameterReportAsync([FromQuery] EquProcessParameterReportPagedQueryDto pagedQueryDto)
        {
            return await _equipmentProcessParameterReportService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 导出设备过程参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        public async Task<EquProcessParameterExportResultDto> ExprotEquProcessParameterListAsync([FromQuery] EquProcessParameterReportPagedQueryDto param)
        {
            return await _equipmentProcessParameterReportService.ExprotEquProcessParameterListAsync(param);
        }

    }
}