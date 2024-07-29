using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquMaintenance.EquMaintenanceItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备保养项目）
    /// @author JAM
    /// @date 2024-05-23 02:12:11
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquMaintenanceItemController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquMaintenanceItemController> _logger;
        /// <summary>
        /// 服务接口（设备保养项目）
        /// </summary>
        private readonly IEquMaintenanceItemService _equMaintenanceItemService;


        /// <summary>
        /// 构造函数（设备保养项目）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equMaintenanceItemService"></param>
        public EquMaintenanceItemController(ILogger<EquMaintenanceItemController> logger, IEquMaintenanceItemService equMaintenanceItemService)
        {
            _logger = logger;
            _equMaintenanceItemService = equMaintenanceItemService;
        }

        /// <summary>
        /// 添加（设备保养项目）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] EquMaintenanceItemSaveDto saveDto)
        {
             await _equMaintenanceItemService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备保养项目）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] EquMaintenanceItemUpdateDto saveDto)
        {
             await _equMaintenanceItemService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备保养项目）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equMaintenanceItemService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（设备保养项目）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquMaintenanceItemDto?> QueryByIdAsync(long id)
        {
            return await _equMaintenanceItemService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（设备保养项目）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquMaintenanceItemDto>> QueryPagedListAsync([FromQuery] EquMaintenanceItemPagedQueryDto pagedQueryDto)
        {
            return await _equMaintenanceItemService.GetPagedListAsync(pagedQueryDto);
        }



    }
}