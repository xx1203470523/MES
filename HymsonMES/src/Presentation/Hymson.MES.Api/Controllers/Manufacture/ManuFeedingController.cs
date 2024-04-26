using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuFeeding;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（物料加载）
    /// </summary>
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
        /// 查询资源（物料加载）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet("resource")]
        public async Task<IEnumerable<SelectOptionDto>> GetFeedingResourceListAsync([FromQuery] ManuFeedingResourceQueryDto queryDto)
        {
            return await _manuFeedingService.GetFeedingResourceListAsync(queryDto);
        }

        /// <summary>
        /// 查工单（物料加载）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet("loadPoint")]
        public async Task<IEnumerable<SelectOptionDto>> GetFeedingLoadPointListAsync([FromQuery] ManuFeedingLoadPointQueryDto queryDto)
        {
            return await _manuFeedingService.GetFeedingLoadPointListAsync(queryDto);
        }

        /// <summary>
        /// 查工单（物料加载）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet("workOder")]
        public async Task<IEnumerable<SelectOptionDto>> GetFeedingWorkOrderListAsync([FromQuery] ManuFeedingWorkOrderQueryDto queryDto)
        {
            return await _manuFeedingService.GetFeedingWorkOrderListAsync(queryDto);
        }

        /// <summary>
        /// 查询物料（物料加载）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet("material")]
        public async Task<IEnumerable<ManuFeedingMaterialDto>> GetFeedingMaterialListAsync([FromQuery] ManuFeedingMaterialQueryDto queryDto)
        {
            return await _manuFeedingService.GetFeedingMaterialListAsync(queryDto);
        }

        /// <summary>
        /// 添加（物料加载）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("物料加载", BusinessType.INSERT)]
        public async Task<ManuFeedingMaterialResponseDto> CreateAsync(ManuFeedingMaterialSaveDto saveDto)
        {
            return await _manuFeedingService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 删除（物料加载）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("物料加载", BusinessType.DELETE)]
        public async Task DeletesAsync(long[] ids)
        {
            await _manuFeedingService.DeletesAsync(ids);
        }

    }
}