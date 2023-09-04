using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Hymson.MES.SystemServices.Dtos.Manufacture;
using Hymson.MES.SystemServices.Dtos.Plan;
using Hymson.MES.SystemServices.Services.Manufacture;
using Hymson.MES.SystemServices.Services.Plan;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace Hymson.MES.System.Api.Controllers
{
    /// <summary>
    /// 系统对接
    /// </summary>
    [Route("SystemService/api/v1/SystemApi")]
    [ApiController]
    public class SystemController : Controller
    {
        private readonly ILogger<SystemController> _logger;
        private readonly IPlanWorkOrderService _planWorkOrderService;
        private readonly IManuSfcCirculationService _manuSfcCirculationService;
        private readonly IProductTraceReportService _productTraceReportService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="planWorkOrderService"></param>
        /// <param name="manuSfcCirculationService"></param>
        /// <param name="productTraceReportService"></param>
        public SystemController(ILogger<SystemController> logger, 
            IPlanWorkOrderService planWorkOrderService, 
            IManuSfcCirculationService manuSfcCirculationService,
            IProductTraceReportService productTraceReportService)
        {
            _logger = logger;
            _planWorkOrderService = planWorkOrderService;
            _manuSfcCirculationService = manuSfcCirculationService;
            _productTraceReportService = productTraceReportService;
        }

        /// <summary>
        /// 工单同步
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("workorderadd")]
        public async Task AddWorkOrderAsync(PlanWorkOrderDto request)
        {
            await _planWorkOrderService.AddWorkOrderAsync(request);
        }

        /// <summary>
        /// 按成品条码/模组条码查询关联层级信息
        /// </summary>
        /// <param name="manuSfcCirculationQueryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("relationship")]
        public async Task<ManuSfcCirculationDto?> GetRelationShipByPackAsync(ManuSfcCirculationQueryDto manuSfcCirculationQueryDto)
        {
            return await _manuSfcCirculationService.GetRelationShipByPackAsync(manuSfcCirculationQueryDto.SFC);
        }

        /// <summary>
        /// 按成品条码/模组条码反查层级信息
        /// </summary>
        /// <param name="manuSfcTraceQueryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("producttrace")]
        public async Task<PagedInfo<Hymson.MES.Services.Dtos.Report.ManuSfcCirculationViewDto>> GetProductTracePagedListAsync([FromQuery] ManuSfcTraceQueryDto manuSfcTraceQueryDto)
        {
            if (string.IsNullOrEmpty(manuSfcTraceQueryDto.SFC)) { throw new CustomerValidationException(nameof(ErrorCode.MES19203)); }
            ProductTracePagedQueryDto productTracePagedQueryDto = new ProductTracePagedQueryDto()
            {
                SFC = manuSfcTraceQueryDto.SFC,
                TraceDirection = false,
                PageSize = 2000,
                PageIndex = 1
            };
            return await _productTraceReportService.GetProductTracePagedListAsync(productTracePagedQueryDto);
        }

        /// <summary>
        /// 按成品条码/模组条码查询参数信息
        /// </summary>
        /// <param name="manuSfcPrameterQueryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("productprameter")]
        public async Task<PagedInfo<ManuProductParameterViewDto>> GetProductPrameterPagedListAsync([FromQuery] ManuSfcPrameterQueryDto manuSfcPrameterQueryDto)
        {
           if (string.IsNullOrEmpty(manuSfcPrameterQueryDto.SFC)) { throw new CustomerValidationException(nameof(ErrorCode.MES19203)); }
            ManuProductPrameterPagedQueryDto param = new ManuProductPrameterPagedQueryDto()
            {
                SFC = manuSfcPrameterQueryDto.SFC,
                ParameterType = ParameterTypeEnum.Product,
                PageSize = 2000,
                PageIndex = 1
            };
            return await _productTraceReportService.GetProductPrameterPagedListAsync(param);
        }

        /// <summary>
        /// 按成品条码/模组条码查询过站信息
        /// </summary>
        /// <param name="manuSfcStepQueryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sfcstep")]
        public async Task<PagedInfo<ManuSfcStepViewDto>> GetSfcStepPagedListAsync([FromQuery] ManuSfcStepQueryDto manuSfcStepQueryDto)
        {
            if (string.IsNullOrEmpty(manuSfcStepQueryDto.SFC)) { throw new CustomerValidationException(nameof(ErrorCode.MES19203)); }
            ManuSfcStepPagedQueryDto param = new ManuSfcStepPagedQueryDto()
            {
                SFC = manuSfcStepQueryDto.SFC,
                PageSize = 2000,
                PageIndex = 1
            };
            return await _productTraceReportService.GetSfcStepPagedListAsync(param);
        }

    }
}