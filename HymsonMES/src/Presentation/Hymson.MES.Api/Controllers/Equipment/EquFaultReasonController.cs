using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备故障原因表）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquFaultReasonController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquFaultReasonController> _logger;

        /// <summary>
        /// 接口（设备故障原因表）
        /// </summary>
        private readonly IEquFaultReasonService _equFaultReasonService;

        /// <summary>
        /// 构造函数（设备故障原因表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equFaultReasonService"></param>
        public EquFaultReasonController(ILogger<EquFaultReasonController> logger,
            IEquFaultReasonService equFaultReasonService)
        {
            _logger = logger;
            _equFaultReasonService = equFaultReasonService;
        }


        /// <summary>
        /// 添加（设备故障原因表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("设备故障原因表", BusinessType.INSERT)]
        [PermissionDescription("equipment:equFaultReason:insert")]
        public async Task AddAsync(EquFaultReasonSaveDto parm)
        {
            await _equFaultReasonService.CreateAsync(parm);
        }

        /// <summary>
        /// 更新（设备故障原因表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("设备故障原因表", BusinessType.UPDATE)]
        [PermissionDescription("equipment:equFaultReason:update")]
        public async Task UpdateAsync(EquFaultReasonSaveDto parm)
        {
            await _equFaultReasonService.ModifyAsync(parm);
        }

        /// <summary>
        /// 删除（设备故障原因表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("设备故障原因表", BusinessType.DELETE)]
        [PermissionDescription("equipment:equFaultReason:delete")]
        public async Task DeleteAsync(long[] ids)
        {
            await _equFaultReasonService.DeletesAsync(ids);
        }

        /// <summary>
        /// 分页查询列表（设备故障原因表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquFaultReasonDto>> GetPagedListAsync([FromQuery] EquFaultReasonPagedQueryDto parm)
        {
            return await _equFaultReasonService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（设备故障原因表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquFaultReasonDto?> QueryByIdAsync(long id)
        {
            return await _equFaultReasonService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 查询ID集合（关联故障原因）
        /// </summary>
        /// <param name="reasonId"></param>
        /// <returns></returns>
        [HttpGet("solutions/{reasonId}")]
        public async Task<IEnumerable<long>> QuerySolutionsByMainIdAsync(long reasonId)
        {
            return await _equFaultReasonService.QuerySolutionsByMainIdAsync(reasonId);
        }

        /// <summary>
        /// 查询列表（关联故障现象）
        /// </summary>
        /// <param name="reasonId"></param>
        /// <returns></returns>
        [HttpGet("phenomenons/{reasonId}")]
        public async Task<IEnumerable<BaseInfoDto>> QueryPhenomenonsByMainIdAsync(long reasonId)
        {
            return await _equFaultReasonService.QueryPhenomenonsByMainIdAsync(reasonId);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IEnumerable<SelectOptionDto>> QueryReasonsAsync()
        {
            return await _equFaultReasonService.QueryReasonsAsync();
        }



        #region 状态变更
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("enable")]
        [PermissionDescription("equipment:equFaultReason:enable")]
        public async Task EnableAsync([FromBody] long id)
        {
            await _equFaultReasonService.UpdateStatusAsync(new ChangeStatusDto
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
        [PermissionDescription("equipment:equFaultReason:retain")]
        public async Task RetainAsyn([FromBody] long id)
        {
            await _equFaultReasonService.UpdateStatusAsync(new ChangeStatusDto
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
        [PermissionDescription("equipment:equFaultReason:abolish")]
        public async Task AbolishAsyn([FromBody] long id)
        {
            await _equFaultReasonService.UpdateStatusAsync(new ChangeStatusDto
            {
                Id = id,
                Status = SysDataStatusEnum.Abolish
            });
        }
        #endregion

    }
}