using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Plan;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Plan
{
    /// <summary>
    /// 控制器（条码接收）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlanSfcReceiveController : ControllerBase
    {
        /// <summary>
        /// 接口（条码接收）
        /// </summary>
        private readonly IPlanSfcReceiveService _planSfcInfoService;
        private readonly ILogger<PlanSfcReceiveController> _logger;

        /// <summary>
        /// 构造函数（条码接收）
        /// </summary>
        /// <param name="planSfcInfoService"></param>
        /// <param name="logger"></param>
        public PlanSfcReceiveController(IPlanSfcReceiveService planSfcInfoService, ILogger<PlanSfcReceiveController> logger)
        {
            _planSfcInfoService = planSfcInfoService;
            _logger = logger;
        }

        /// <summary>
        /// 添加（条码接收）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("receive")]
        [LogDescription("条码接收", BusinessType.INSERT)]
        [PermissionDescription("plan:sfcReceive:receive")]
        public async Task AddPlanSfcInfoAsync([FromBody] PlanSfcReceiveCreateDto parm)
        {
            await _planSfcInfoService.CreatePlanSfcInfoAsync(parm);
        }

        /// <summary>
        /// 分页查询列表（条码接收）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("scanCode")]
        public async Task<PlanSfcReceiveSfcDto> ScanCodeInfoAsync([FromQuery] PlanSfcReceiveScanCodeDto parm)
        {
            return await _planSfcInfoService.PlanSfcReceiveScanCodeAsync(parm);
        }

        /// <summary>
        /// 批量查询条码信息（条码接收）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("scanCodeList")]
        public async Task<IEnumerable<PlanSfcReceiveSfcDto>> ScanCodeListAsync([FromBody] PlanSfcReceiveScanListDto parm)
        {
            return await _planSfcInfoService.PlanSfcReceiveScanListAsync(parm);
        }
    }
}   