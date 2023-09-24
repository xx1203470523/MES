using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.BindSFC;
using Hymson.MES.EquipmentServices.Services.BindSFC;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuSFCBindController : ControllerBase
    {
        private readonly IBindSFCService _bindSFCService;
        private readonly ILogger<ManuSfcProduceController> _logger;

        public ManuSFCBindController(
            IBindSFCService bindSFCService,
            ILogger<ManuSfcProduceController> logger)
        {
            _bindSFCService = bindSFCService;
            _logger = logger;
        }

        /// <summary>
        /// 获取SFC绑定数据(永泰维修)
        /// <para>PS：这里没法获取当前位置吧，我看原型的意思应该是当前SFC的工序吧</para>
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pda/pagelist")]
        public async Task<BindSFCOutputDto> QuerySFCBindAsync([FromQuery] BindSFCInputDto parm)
        {
            return await _bindSFCService.GetBindSFC(parm);
        }

        /// <summary>
        /// 全部解绑
        /// <para>PS：我没传BindSFCs，这个应该是你通过SFC（SFC是产线编码的意思不？）在后台查，不应该让我来传，不然我传些非本SFC下的设备不会出问题么</para>
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pda/UnBind")]
        public async Task UnBindSFCAsync(UnBindSFCInputDto parm)
        {
            await _bindSFCService.UnBindSFCAsync(parm);
        }

        /// <summary>
        /// 换绑
        /// <para>PS：对接完成了，但是有BUG，我感觉应该传旧的设备绑定ID，而不是旧的SFC，如果即将换绑的新SFC设备码和旧SFC设备码相同，就会出BUG</para>
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pda/switchBind")]
        public async Task SwitchBindSFCAsync(SwitchBindInputDto parm)
        {
            await _bindSFCService.SwitchBindSFCAsync(parm);
        }

        /// <summary>
        /// 复投
        /// <para>PS；传参我改变了一下，按原型来看，我应该是只需要传条码、NG位置、复投位置就可以了</para>
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pda/repeatManu")]
        public async Task RepeatManuSFCAsync(ResumptionInputDto parm)
        {
            //await _bindSFCService.RepeatManuSFCAsync(parm);
        }



    }
}
