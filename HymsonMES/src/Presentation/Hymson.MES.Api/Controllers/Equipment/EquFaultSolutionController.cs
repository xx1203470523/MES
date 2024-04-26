using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备故障解决措施）
    /// @author Czhipu
    /// @date 2023-12-19 07:11:01
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquFaultSolutionController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquFaultSolutionController> _logger;

        /// <summary>
        /// 服务接口（设备故障解决措施）
        /// </summary>
        private readonly IEquFaultSolutionService _equFaultSolutionService;

        /// <summary>
        /// 构造函数（设备故障解决措施）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equFaultSolutionService"></param>
        public EquFaultSolutionController(ILogger<EquFaultSolutionController> logger,
            IEquFaultSolutionService equFaultSolutionService)
        {
            _logger = logger;
            _equFaultSolutionService = equFaultSolutionService;
        }

        /// <summary>
        /// 添加（设备故障解决措施）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("设备故障解决措施", BusinessType.INSERT)]
        [PermissionDescription("equipment:equFaultSolution:insert")]
        public async Task AddAsync([FromBody] EquFaultSolutionSaveDto saveDto)
        {
            await _equFaultSolutionService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备故障解决措施）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("设备故障解决措施", BusinessType.UPDATE)]
        [PermissionDescription("equipment:equFaultSolution:update")]
        public async Task UpdateAsync([FromBody] EquFaultSolutionSaveDto saveDto)
        {
            await _equFaultSolutionService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备故障解决措施）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("设备故障解决措施", BusinessType.DELETE)]
        [PermissionDescription("equipment:equFaultSolution:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equFaultSolutionService.DeletesAsync(ids);
        }

        /// <summary>
        /// 分页查询列表（设备故障解决措施）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquFaultSolutionDto>> GetPagedListAsync([FromQuery] EquFaultSolutionPagedQueryDto pagedQueryDto)
        {
            return await _equFaultSolutionService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（设备故障解决措施）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquFaultSolutionDto?> QueryByIdAsync(long id)
        {
            return await _equFaultSolutionService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 查询列表（关联故障原因）
        /// </summary>
        /// <param name="solutionId"></param>
        /// <returns></returns>
        [HttpGet("reasons/{solutionId}")]
        public async Task<IEnumerable<BaseInfoDto>> QueryReasonsByMainIdAsync(long solutionId)
        {
            return await _equFaultSolutionService.QueryReasonsByMainIdAsync(solutionId);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IEnumerable<SelectOptionDto>> QuerySolutionsAsync()
        {
            return await _equFaultSolutionService.QuerySolutionsAsync();
        }



        #region 状态变更
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("enable")]
        [PermissionDescription("equipment:equFaultSolution:enable")]
        public async Task EnableAsync([FromBody] long id)
        {
            await _equFaultSolutionService.UpdateStatusAsync(new ChangeStatusDto
            {
                Id = id,
                Status = SysDataStatusEnum.Enable
            });
        }

        /// <summary>
        /// 保留
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("retain")]
        [PermissionDescription("equipment:equFaultSolution:retain")]
        public async Task RetainAsyn([FromBody] long id)
        {
            await _equFaultSolutionService.UpdateStatusAsync(new ChangeStatusDto
            {
                Id = id,
                Status = SysDataStatusEnum.Retain
            });
        }

        /// <summary>
        /// 废除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("abolish")]
        [PermissionDescription("equipment:equFaultSolution:abolish")]
        public async Task AbolishAsyn([FromBody] long id)
        {
            await _equFaultSolutionService.UpdateStatusAsync(new ChangeStatusDto
            {
                Id = id,
                Status = SysDataStatusEnum.Abolish
            });
        }
        #endregion

    }
}