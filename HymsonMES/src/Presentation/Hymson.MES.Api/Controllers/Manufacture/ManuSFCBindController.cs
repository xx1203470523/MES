using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.BindSFC;
using Hymson.MES.EquipmentServices.Services.BindSFC;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuSfcProduce;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuSFCBindController : ControllerBase
    {
        private readonly IBindSFCService _bindSFCService;
        private readonly ILogger<ManuSfcProduceController> _logger;

        public ManuSFCBindController(IBindSFCService bindSFCService, ILogger<ManuSfcProduceController> logger)
        {
            _bindSFCService = bindSFCService;
            _logger = logger;
        }

        /// <summary>
        /// 获取SFC绑定数据(永泰维修)
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pda/pagelist")]
        public async Task<IEnumerable<ManuSfcBindEntity>> QuerySFCBindAsync([FromQuery] BindSFCDto parm)
        {
            return await _bindSFCService.GetBindSFC(parm);
        }

        /// <summary>
        /// 全部解绑
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pda/UnBind")]
        public async Task UnBindSFCAsync(UnBindSFCDto parm)
        {
            await _bindSFCService.UnBindSFCAsync(parm);
        }

        /// <summary>
        /// 换绑
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pda/switchBind")]
        public async Task SwitchBindSFCAsync(SwitchBindSFCDto parm)
        {
            await _bindSFCService.SwitchBindSFCAsync(parm);
        }

        /// <summary>
        /// 复投
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pda/repeatManu")]
        public async Task RepeatManuSFCAsync(UnBindSFCDto parm)
        {
            await _bindSFCService.RepeatManuSFCAsync(parm);
        }



    }
}
