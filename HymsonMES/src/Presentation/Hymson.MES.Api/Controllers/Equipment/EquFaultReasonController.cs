using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
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
        /// 接口（设备故障原因表）
        /// </summary>
        private readonly IEquFaultReasonService _EquFaultReasonService;
        private readonly ILogger<EquFaultReasonController> _logger;

        /// <summary>
        /// 构造函数（设备故障原因表）
        /// </summary>
        /// <param name="EquFaultReasonService"></param>
        /// <param name="logger"></param>
        public EquFaultReasonController(IEquFaultReasonService EquFaultReasonService, ILogger<EquFaultReasonController> logger)
        {
            _EquFaultReasonService = EquFaultReasonService;
            _logger = logger;
        }


        /// <summary>
        /// 添加（设备故障原因表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("设备故障原因", BusinessType.INSERT)]
        [PermissionDescription("equipment:equFaultReason:insert")]
        public async Task AddEquFaultReasonAsync(EquFaultReasonSaveDto parm)
        {
            await _EquFaultReasonService.CreateEquFaultReasonAsync(parm);
        }

        /// <summary>
        /// 更新（设备故障原因表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("设备故障原因", BusinessType.UPDATE)]
        [PermissionDescription("equipment:equFaultReason:update")]
        public async Task UpdateEquFaultReasonAsync(EquFaultReasonSaveDto parm)
        {
            await _EquFaultReasonService.ModifyEquFaultReasonAsync(parm);
        }

        /// <summary>
        /// 删除（设备故障原因表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("设备故障原因", BusinessType.DELETE)]
        [PermissionDescription("equipment:equFaultReason:delete")]
        public async Task DeleteEquFaultReasonAsync(long[] ids)
        {
            await _EquFaultReasonService.DeletesEquFaultReasonAsync(ids);
        }

        /// <summary>
        /// 分页查询列表（设备故障原因表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquFaultReasonDto>> QueryPagedEquFaultReasonAsync([FromQuery] EquFaultReasonPagedQueryDto parm)
        {
            return await _EquFaultReasonService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询列表（设备故障原因表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getlist")]
        public async Task<IEnumerable<EquFaultReasonDto>> QueryEquFaultReasonListAsync([FromQuery] EquFaultReasonQueryDto parm)
        {
            return await _EquFaultReasonService.GetListAsync(parm);
        }

        /// <summary>
        /// 查询详情（设备故障原因表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquFaultReasonDto> QueryEquFaultReasonByIdAsync(long id)
        {
            return await _EquFaultReasonService.QueryEquFaultReasonByIdAsync(id);
        }

        #region 状态变更
        /// <summary>
        /// 启用（设备故障原因表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("设备故障原因表", BusinessType.UPDATE)]
        [PermissionDescription("equipment:equFaultReason:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _EquFaultReasonService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（设备故障原因表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("设备故障原因表", BusinessType.UPDATE)]
        [PermissionDescription("equipment:equFaultReason:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _EquFaultReasonService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（设备故障原因表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("设备故障原因表", BusinessType.UPDATE)]
        [PermissionDescription("equipment:equFaultReason:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _EquFaultReasonService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}