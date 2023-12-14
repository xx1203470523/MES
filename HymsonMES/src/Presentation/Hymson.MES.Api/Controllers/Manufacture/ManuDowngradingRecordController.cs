using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（降级品录入记录）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuDowngradingRecordController : ControllerBase
    {
        /// <summary>
        /// 接口（降级品录入记录）
        /// </summary>
        private readonly IManuDowngradingRecordService _manuDowngradingRecordService;
        private readonly ILogger<ManuDowngradingRecordController> _logger;

        /// <summary>
        /// 构造函数（降级品录入记录）
        /// </summary>
        /// <param name="manuDowngradingRecordService"></param>
        /// <param name="logger"></param>
        public ManuDowngradingRecordController(IManuDowngradingRecordService manuDowngradingRecordService, ILogger<ManuDowngradingRecordController> logger)
        {
            _manuDowngradingRecordService = manuDowngradingRecordService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（降级品录入记录）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuDowngradingRecordDto>> QueryPagedManuDowngradingRecordAsync([FromQuery] ManuDowngradingRecordPagedQueryDto parm)
        {
            return await _manuDowngradingRecordService.GetPagedListAsync(parm);
        }

        #endregion
    }
}