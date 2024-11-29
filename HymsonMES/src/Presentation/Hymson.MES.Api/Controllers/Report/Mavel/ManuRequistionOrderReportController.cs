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
    public class ManuRequistionOrderReportController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuRequistionOrderReportController> _logger;
        /// <summary>
        /// 服务接口（领料记录详情）
        /// </summary>
        private readonly IManuRequistionOrderReportService _manuRequistionOrderReportService;

        /// <summary>
        /// 构造函数（领料记录详情）
        /// </summary>
        public ManuRequistionOrderReportController(ILogger<ManuRequistionOrderReportController> logger,
            IManuRequistionOrderReportService manuRequistionOrderReportService)
        {
            _logger = logger;
            _manuRequistionOrderReportService = manuRequistionOrderReportService;
        }

        /// <summary>
        /// 分页查询列表（领料记录详情）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        [AllowAnonymous]
        public async Task<PagedInfo<ReportRequistionOrderResultDto>> QueryPagedListAsync([FromQuery] ReportRequistionOrderQueryDto pagedQueryDto)
        {
            return await _manuRequistionOrderReportService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询仓库地址分组
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("warehouselist")]
        [AllowAnonymous]
        public async Task<List<ManuRequistionOrderGroupDto>> GetWarehouseListAsync()
        {
            return await _manuRequistionOrderReportService.GetWarehouseListAsync();
        }

        /// <summary>
        /// 查询仓库地址分组Code
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("warehousecodelist")]
        [AllowAnonymous]
        public async Task<List<ManuRequistionOrderCodeGroupDto>> GetWarehouseCodeListAsync()
        {
            return await _manuRequistionOrderReportService.GetWarehouseCodeListAsync();
        }

        /// <summary>
        /// 导出信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        [PermissionDescription("report:manuRequistionOrderReport:export")]
        public async Task<NioPushCollectionExportResultDto> ExprotComUsagePageListAsync([FromQuery] ReportRequistionOrderQueryDto param)
        {
            return await _manuRequistionOrderReportService.ExprotAsync(param);
        }

    }
}