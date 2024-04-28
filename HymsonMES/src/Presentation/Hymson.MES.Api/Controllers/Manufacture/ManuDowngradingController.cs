using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（降级录入）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuDowngradingController : ControllerBase
    {
        /// <summary>
        /// 接口（降级录入）
        /// </summary>
        private readonly IManuDowngradingService _manuDowngradingService;
        private readonly ILogger<ManuDowngradingController> _logger;

        /// <summary>
        /// 构造函数（降级录入）
        /// </summary>
        /// <param name="manuDowngradingService"></param>
        /// <param name="logger"></param>
        public ManuDowngradingController(IManuDowngradingService manuDowngradingService, ILogger<ManuDowngradingController> logger)
        {
            _manuDowngradingService = manuDowngradingService;
            _logger = logger;
        }

        /// <summary>
        /// 根据sfcs查询详情（降级录入）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost("getDowngradingBySfcs")]
        public async Task<IEnumerable<ManuDowngradingDto>> QueryManuDowngradingBySfcsAsync(ManuDowngradingQuerySfcsDto queryDto)
        {
            return await _manuDowngradingService.GetManuDowngradingBySfcsAsync(queryDto.Sfcs);
        }

        /// <summary>
        /// 保存（降级录入）
        /// </summary>
        /// <param name="manuDowngradingSaveDto"></param>
        /// <returns></returns>
        [HttpPost]        
        [Route("saveManuDowngrading")]
        [LogDescription("降级录入", BusinessType.INSERT)]
        [PermissionDescription("manu:downgrading:save")]
        public async Task SaveManuDowngradingAsync([FromBody] ManuDowngradingSaveDto manuDowngradingSaveDto)
        {
             await _manuDowngradingService.SaveManuDowngradingAsync(manuDowngradingSaveDto);
        }

        /// <summary>
        /// 保存:进行降级移除
        /// </summary>
        /// <param name="manuDowngradingSaveRemoveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("saveManuDowngradingRemove")]
        [LogDescription("进行降级移除", BusinessType.INSERT)]
        [PermissionDescription("manu:downgrading:saveRemove")]
        public async Task SaveManuDowngradingRemoveAsync([FromBody] ManuDowngradingSaveRemoveDto manuDowngradingSaveRemoveDto)
        {
            await _manuDowngradingService.SaveManuDowngradingRemoveAsync(manuDowngradingSaveRemoveDto);
        }
    }
}