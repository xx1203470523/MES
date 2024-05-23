using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment.EquMaintenance;
using Hymson.MES.Services.Services.Equipment.EquMaintenance.EquMaintenanceTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备保养任务）
    /// @author JAM
    /// @date 2024-05-23 03:20:49
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquMaintenanceTaskController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquMaintenanceTaskController> _logger;
        /// <summary>
        /// 服务接口（设备保养任务）
        /// </summary>
        private readonly IEquMaintenanceTaskService _equMaintenanceTaskService;


        /// <summary>
        /// 构造函数（设备保养任务）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equMaintenanceTaskService"></param>
        public EquMaintenanceTaskController(ILogger<EquMaintenanceTaskController> logger, IEquMaintenanceTaskService equMaintenanceTaskService)
        {
            _logger = logger;
            _equMaintenanceTaskService = equMaintenanceTaskService;
        }

        /// <summary>
        /// 添加（设备保养任务）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] EquMaintenanceTaskSaveDto saveDto)
        {
             await _equMaintenanceTaskService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备保养任务）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] EquMaintenanceTaskSaveDto saveDto)
        {
             await _equMaintenanceTaskService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备保养任务）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equMaintenanceTaskService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（设备保养任务）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquMaintenanceTaskDto?> QueryByIdAsync(long id)
        {
            return await _equMaintenanceTaskService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（设备保养任务）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquMaintenanceTaskDto>> QueryPagedListAsync([FromQuery] EquMaintenanceTaskPagedQueryDto pagedQueryDto)
        {
            return await _equMaintenanceTaskService.GetPagedListAsync(pagedQueryDto);
        }

    }
}