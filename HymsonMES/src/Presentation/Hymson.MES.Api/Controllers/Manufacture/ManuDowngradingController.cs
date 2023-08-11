/*
 *creator: Karl
 *
 *describe: 降级录入    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-08-10 10:15:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（降级录入）
    /// @author Karl
    /// @date 2023-08-10 10:15:26
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
        public ManuDowngradingController(IManuDowngradingService manuDowngradingService, ILogger<ManuDowngradingController> logger)
        {
            _manuDowngradingService = manuDowngradingService;
            _logger = logger;
        }

        ///// <summary>
        ///// 根据sfcs查询详情（降级录入）
        ///// </summary>
        ///// <param name="queryDto"></param>
        ///// <returns></returns>
        //[HttpGet("getDowngradingBySfcs")]
        //public async Task<IEnumerable<ManuDowngradingDto>> QueryManuDowngradingBySfcsAsync([FromQuery] ManuDowngradingQuerySfcsDto queryDto)
        //{
        //    return await _manuDowngradingService.GetManuDowngradingBySfcsAsync(queryDto.Sfcs);
        //}

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
        public async Task SaveManuDowngradingAsync([FromBody] ManuDowngradingSaveDto manuDowngradingSaveDto)
        {
             await _manuDowngradingService.SaveManuDowngradingAsync(manuDowngradingSaveDto);
        }

    }
}