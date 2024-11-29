using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.NioPushCollection;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（领料记录详情）
    /// @author Yxx
    /// @date 2024-10-25 02:08:27
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuReturnOrderReportController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuRequistionOrderReportController> _logger;
        /// <summary>
        /// 服务接口（领料记录详情）
        /// </summary>
        private readonly IManuReturnOrderReportService _manuReturnOrderReportService;

        /// <summary>
        /// 构造函数（领料记录详情）
        /// </summary>
        public ManuReturnOrderReportController(ILogger<ManuRequistionOrderReportController> logger,
            IManuReturnOrderReportService manuReturnOrderReportService)
        {
            _logger = logger;
            _manuReturnOrderReportService = manuReturnOrderReportService;
        }

        /// <summary>
        /// 分页查询列表（领料记录详情）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        [AllowAnonymous]
        public async Task<PagedInfo<ReportReturnOrderResultDto>> QueryPagedListAsync([FromQuery] ReportReturnOrderQueryDto pagedQueryDto)
        {
            return await _manuReturnOrderReportService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 导出信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        [PermissionDescription("report:manuReturnOrderReport:export")]
        public async Task<NioPushCollectionExportResultDto> ExprotComUsagePageListAsync([FromQuery] ReportReturnOrderQueryDto param)
        {
            return await _manuReturnOrderReportService.ExprotAsync(param);
        }

    }
}