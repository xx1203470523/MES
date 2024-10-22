using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（操作面板-生产过站面板）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuFacePlateProductionController : ControllerBase
    {
        /// <summary>
        /// 接口（操作面板）
        /// </summary>
        private readonly IManuFacePlateProductionService _manuFacePlateProductionService;
        private readonly ILogger<ManuFacePlateController> _logger;

        /// <summary>
        /// 构造函数（操作面板）
        /// </summary>
        /// <param name="manuFacePlateProductionService"></param>    
        /// <param name="logger"></param>   
        public ManuFacePlateProductionController(IManuFacePlateProductionService manuFacePlateProductionService, ILogger<ManuFacePlateController> logger)
        {
            _manuFacePlateProductionService = manuFacePlateProductionService;
            _logger = logger;
        }

        /// <summary>
        /// 组装界面获取当前条码对应bom下 当前需要组装的物料信息（操作面板）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("getPlateProductionPackageInfo")]
        public async Task<ManuFacePlateProductionPackageDto> GetsManuFacePlateProductionPackageInfo([FromQuery] ManuFacePlateProductionPackageQueryDto param)
        {
            return await _manuFacePlateProductionService.GetManuFacePlateProductionPackageInfoAsync(param);
        }
    }
}