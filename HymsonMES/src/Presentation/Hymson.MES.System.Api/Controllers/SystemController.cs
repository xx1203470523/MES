using Hymson.MES.SystemServices.Dtos.Manufacture;
using Hymson.MES.SystemServices.Dtos.Plan;
using Hymson.MES.SystemServices.Services.Manufacture;
using Hymson.MES.SystemServices.Services.Plan;
using Microsoft.AspNetCore.Mvc;

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

        public SystemController(ILogger<SystemController> logger, IPlanWorkOrderService planWorkOrderService, IManuSfcCirculationService manuSfcCirculationService)
        {
            _logger = logger;
            _planWorkOrderService = planWorkOrderService;
            _manuSfcCirculationService = manuSfcCirculationService;
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

    }
}