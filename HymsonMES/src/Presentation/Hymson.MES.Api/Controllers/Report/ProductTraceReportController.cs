using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Plan;
using Hymson.MES.Services.Services.Report;
using Hymson.MES.Services.Services.Report.ManuProductParameterReport;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 产品追溯
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductTraceReportController : ControllerBase
    {
        private readonly IProductTraceReportService _productTraceReportService;
        public ProductTraceReportController(IProductTraceReportService productTraceReportService)
        {
            _productTraceReportService = productTraceReportService;
        }
        /// <summary>
        /// 工单查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("workorder")]
        public async Task<PagedInfo<ProductTracePlanWorkOrderViewDto>> GetWorkOrderPagedListAsync([FromQuery] ProductTracePlanWorkOrderPagedQueryDto param)
        {
            return await _productTraceReportService.GetWorkOrderPagedListAsync(param);
        }
        /// <summary>
        /// 追溯查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("producttrace")]
        public async Task<PagedInfo<ManuSfcCirculationViewDto>> GetProductTracePagedListAsync([FromQuery] ProductTracePagedQueryDto param)
        {
            return await _productTraceReportService.GetProductTracePagedListAsync(param);
        }

        /// <summary>
        /// 参数查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("productprameter")]
        public async Task<PagedInfo<ManuProductParameterViewDto>> GetProductPrameterPagedListAsync([FromQuery] ManuProductPrameterPagedQueryDto param)
        {
            return await _productTraceReportService.GetProductPrameterPagedListAsync(param);
        }

        /// <summary>
        /// 条码履历
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("sfcstep")]
        public async Task<PagedInfo<ManuSfcStepViewDto>> GetSfcStepPagedListAsync([FromQuery] ManuSfcStepPagedQueryDto param)
        {
            return await _productTraceReportService.GetSfcStepPagedListAsync(param);
        }

        /// <summary>
        /// 条码生产工艺
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("sfcprocessroute")]
        public async Task<PagedInfo<ProcSfcProcessRouteViewDto>> GetSfcProcessRoutePagedListAsync([FromQuery] ProcSfcProcessRoutePagedQueryDto param)
        {
            return await _productTraceReportService.GetSfcProcessRoutePagedListAsync(param);
        }

        /// <summary>
        /// 产品追朔导出
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("export")]
        public async Task<ExportResultDto> ManuProductParameterReportExportAsync([FromQuery] ProductTracePagedQueryDto param)
        {
            return await _productTraceReportService.ProductTracingReportExportAsync(param);
        }
    }
}
