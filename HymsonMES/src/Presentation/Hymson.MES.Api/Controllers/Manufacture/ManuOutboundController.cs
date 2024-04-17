using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperateDto;
using Hymson.MES.Services.Services.Manufacture.ManuOutbound;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（条码生产信息（物理删除））
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuOutboundController : ControllerBase
    {
        /// <summary>
        /// 接口（条码生产信息（物理删除））
        /// </summary>
        private readonly IManuOutboundService _manuOutboundService;

        /// <summary>
        /// 构造函数（条码生产信息（物理删除））
        /// </summary>
        /// <param name="manuOutboundService"></param>
        public ManuOutboundController(IManuOutboundService manuOutboundService)
        {
            _manuOutboundService = manuOutboundService;
        }

        /// <summary>
        /// 产出确认
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("outputConfirm")]
        //[PermissionDescription("qual:qualityLock:lock")]
        [LogDescription("产出确认", BusinessType.OTHER, "OutputConfirm", ReceiverTypeEnum.MES)]
        public async Task<Dictionary<string, JobResponseDto>> OutputConfirmAsync(OutputConfirmDto parm)
        {
            return await _manuOutboundService.OutputConfirmAsync(parm);
        }
    }
}