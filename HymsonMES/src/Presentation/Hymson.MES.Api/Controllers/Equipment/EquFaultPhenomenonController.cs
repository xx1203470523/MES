using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquFaultPhenomenon;
using Hymson.Utils;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备故障现象）
    /// @author Czhipu
    /// @date 2023-02-15 08:56:34
    /// </summary>
    
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquFaultPhenomenonController : ControllerBase
    {
        /// <summary>
        /// 接口（设备故障现象）
        /// </summary>
        private readonly IEquFaultPhenomenonService _equFaultPhenomenonService;
        private readonly ILogger<EquFaultPhenomenonController> _logger;

        /// <summary>
        /// 构造函数（设备故障现象）
        /// </summary>
        /// <param name="equFaultPhenomenonService"></param>
        /// <param name="logger"></param>
        public EquFaultPhenomenonController(IEquFaultPhenomenonService equFaultPhenomenonService, ILogger<EquFaultPhenomenonController> logger)
        {
            _equFaultPhenomenonService = equFaultPhenomenonService;
            _logger = logger;
        }


        /// <summary>
        /// 添加（设备故障现象）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("设备故障现象", BusinessType.INSERT)]
        [PermissionDescription("equ:faultPhenomenon:insert")]
        public async Task CreateAsync(EquFaultPhenomenonSaveDto createDto)
        {
            await _equFaultPhenomenonService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（设备故障现象）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("设备故障现象", BusinessType.UPDATE)]
        [PermissionDescription("equ:faultPhenomenon:update")]
        public async Task ModifyAsync(EquFaultPhenomenonSaveDto modifyDto)
        {
            await _equFaultPhenomenonService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（设备故障现象）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("设备故障现象", BusinessType.DELETE)]
        [PermissionDescription("equ:faultPhenomenon:delete")]
        public async Task DeletesAsync(long[] ids)
        {
            await _equFaultPhenomenonService.DeletesAsync(ids);
        }

        /// <summary>
        /// 分页查询列表（设备故障现象）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        //[PermissionDescription("equ:faultPhenomenon:list")]
        public async Task<PagedInfo<EquFaultPhenomenonDto>> GetPagedListAsync([FromQuery] EquFaultPhenomenonPagedQueryDto pagedQueryDto)
        {
            return await _equFaultPhenomenonService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquFaultPhenomenonDto> GetDetailAsync(long id)
        {
            return await _equFaultPhenomenonService.GetDetailAsync(id);
        }

        #region 状态变更
        /// <summary>
        /// 启用（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("设备故障现象", BusinessType.UPDATE)]
        [PermissionDescription("equ:faultPhenomenon:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _equFaultPhenomenonService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("设备故障现象", BusinessType.UPDATE)]
        [PermissionDescription("equ:faultPhenomenon:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _equFaultPhenomenonService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("设备故障现象", BusinessType.UPDATE)]
        [PermissionDescription("equ:faultPhenomenon:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _equFaultPhenomenonService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}