using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuFeeding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（物料加载）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuFeedingController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IManuFeedingService _manuFeedingService;
        private readonly ILogger<ManuFeedingController> _logger;

        /// <summary>
        /// 构造函数（物料加载）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuFeedingService"></param>
        public ManuFeedingController(ILogger<ManuFeedingController> logger, IManuFeedingService manuFeedingService)
        {
            _logger = logger;
            _manuFeedingService = manuFeedingService;
        }

        /// <summary>
        /// 查询类型（物料加载）
        /// </summary>
        /// <param name="code">设备编码/资源编码</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        public async Task<FeedingSourceEnum> GetFeedingSourceAsync(string code)
        {
            return await _manuFeedingService.GetFeedingSourceAsync(code);
        }

        /// <summary>
        /// 查询物料（物料加载）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IEnumerable<ManuFeedingMaterialDto>> GetFeedingListeAsync([FromQuery] ManuFeedingMaterialQueryDto queryDto)
        {
            return await _manuFeedingService.GetFeedingListeAsync(queryDto);
        }

        /*
        /// <summary>
        /// 添加（物料加载）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task CreateAsync(InteContainerSaveDto createDto)
        {
            await _inteContainerService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（容器维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task ModifyAsync(InteContainerSaveDto modifyDto)
        {
            await _inteContainerService.ModifyAsync(modifyDto);
        }
        */
    }
}