using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.BindSFC;
using Hymson.MES.EquipmentServices.Services.BindSFC;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuSFCBindController : ControllerBase
    {
        private readonly IBindSFCService _bindSFCService;
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        private readonly IManuSfcCirculationService _manuSfcCirculationService;
        private readonly ILogger<ManuSfcProduceController> _logger;

        public ManuSFCBindController(
            IBindSFCService bindSFCService,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuSfcCirculationService manuSfcCirculationService,
            IManuSfcInfoRepository manuSfcInfoRepository,
            ILogger<ManuSfcProduceController> logger)
        {
            _bindSFCService = bindSFCService;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuSfcCirculationService = manuSfcCirculationService;
            _logger = logger;
        }

        /// <summary>
        /// 获取SFC绑定数据(永泰维修)       
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pda/pagelist")]
        public async Task<BindSFCOutputDto> QuerySFCBindAsync([FromQuery]BindSFCInputDto parm)
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

        /// <summary>
        /// 获取条码绑定信息
        /// </summary>
        /// <param name="Sfc"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pda/bindsfc/get/{Sfc}")]
        public async Task<IEnumerable<ManuSfcCirculationOutputDto>> GetSfcCirculationBySFCAsync(string Sfc)
        {
            return await _manuSfcCirculationService.GetManuSfcCirculationBySFCAsync(Sfc);
        }

        /// <summary>
        /// 删除条码绑定信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pda/bindsfc/del")]
        public async Task<int> DeteleteManuSfcCirculationAsync([FromBody]long Id)
        {
            return await _manuSfcCirculationService.DeteleteManuSfcCirculationAsync(Id);
        }

        [HttpPost]
        [Route("pda/bindsfc/add")]
        public async Task AddManuSfcCirculationAsync([FromBody] ManuSfcCirculationCreateDto createDto)
        {
            //TODO 绑定涉及几个场景，需要增加查询条件
            await Task.CompletedTask;
        }
    }
}
