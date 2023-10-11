using Hymson.MES.Core.Enums;
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
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pda/pagelist")]
        public async Task<BindSFCOutputDto> QuerySFCBindAsync(BindSFCInputDto parm)
        {
            return await _bindSFCService.GetBindSFC(parm);
        }

        /// <summary>
        /// 永泰维修-状态确认
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pda/repairok")]
        public async Task<BindSFCOutputDto> RepairSFCAsync([FromQuery] BindSFCInputDto parm)
        {
            parm.OperateType = RepairOperateTypeEnum.OK;
            return await _bindSFCService.GetBindSFC(parm);
        }

        /// <summary>
        /// 全部解绑      
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pda/UnBind")]
        public async Task UnBindSFCAsync(UnBindSFCInput parm)
        {
            await _bindSFCService.UnBindPDAAsync(parm);
        }

        /// <summary>
        /// 换绑   
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
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pda/repeatManu")]
        public async Task RepeatManuSFCAsync(ResumptionInputDto parm)
        {
            await _bindSFCService.RepeatManuSFCAsync(parm);
        }



    }
}
