using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（点检任务）
    /// @author User
    /// @date 2024-04-03 04:51:32
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquInspectionTaskController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquInspectionTaskController> _logger;
        /// <summary>
        /// 服务接口（点检任务）
        /// </summary>
        private readonly IEquInspectionTaskService _equInspectionTaskService;

        /// <summary>
        /// 构造函数（点检任务）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equInspectionTaskService"></param>
        public EquInspectionTaskController(ILogger<EquInspectionTaskController> logger, IEquInspectionTaskService equInspectionTaskService)
        {
            _logger = logger;
            _equInspectionTaskService = equInspectionTaskService;
        }

        /// <summary>
        /// 添加（点检任务）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("点检任务", BusinessType.INSERT)]
        [PermissionDescription("equ:equInspectionTask:insert")]
        public async Task AddAsync([FromBody] EquInspectionTaskSaveDto saveDto)
        {
            await _equInspectionTaskService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（点检任务）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("点检任务", BusinessType.UPDATE)]
        [PermissionDescription("equ:equInspectionTask:update")]
        public async Task UpdateAsync([FromBody] EquInspectionTaskSaveDto saveDto)
        {
            await _equInspectionTaskService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（点检任务）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("点检任务", BusinessType.DELETE)]
        [PermissionDescription("equ:equInspectionTask:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equInspectionTaskService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（点检任务）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquInspectionTaskDto?> QueryByIdAsync(long id)
        {
            return await _equInspectionTaskService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（点检任务）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquInspectionTaskDto>> QueryPagedListAsync([FromQuery] EquInspectionTaskPagedQueryDto pagedQueryDto)
        {
            return await _equInspectionTaskService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询点检任务详情
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("detail/{taskId}")]
        public async Task<IEnumerable<EquInspectionTaskDetailDto>> QueryEquipmentsByResourceIdAsync(long taskId)
        {
            return await _equInspectionTaskService.QueryEquipmentsByResourceIdAsync(taskId);
        }

        #region 状态变更
        /// <summary>
        /// 启用（工序维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("点检任务", BusinessType.UPDATE)]
        [PermissionDescription("equ:equInspectionTask:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _equInspectionTaskService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（工序维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("点检任务", BusinessType.UPDATE)]
        [PermissionDescription("equ:equInspectionTask:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _equInspectionTaskService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（工序维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("点检任务", BusinessType.UPDATE)]
        [PermissionDescription("equ:equInspectionTask:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _equInspectionTaskService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}