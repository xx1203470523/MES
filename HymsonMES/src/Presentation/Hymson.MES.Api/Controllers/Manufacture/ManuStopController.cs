using Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperate;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（中止生产）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuStopController : ControllerBase
    {
        /// <summary>
        /// 生产服务接口
        /// </summary>
        private readonly IManuSfcOperateService _manuSfcOperateService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuSfcOperateService"></param>
        public ManuStopController(IManuSfcOperateService manuSfcOperateService)
        {
            _manuSfcOperateService = manuSfcOperateService;
        }

        /// <summary>
        /// 中止（多个）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("StopStationMore")]
        [LogDescription("中止（多个）", BusinessType.OTHER, "StopStationMore", ReceiverTypeEnum.MES)]
        public async Task StopStationMoreAsync(StopBoundDto request)
        {
            await _manuSfcOperateService.StopStationMoreAsync(request);
        }


    }
}