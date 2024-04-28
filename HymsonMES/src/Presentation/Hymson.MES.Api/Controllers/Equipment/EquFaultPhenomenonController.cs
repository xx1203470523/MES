using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquFaultPhenomenon;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备故障现象）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquFaultPhenomenonController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquFaultPhenomenonController> _logger;

        /// <summary>
        /// 接口（设备故障现象）
        /// </summary>
        private readonly IEquFaultPhenomenonService _equFaultPhenomenonService;

        /// <summary>
        /// 构造函数（设备故障现象）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equFaultPhenomenonService"></param>
        public EquFaultPhenomenonController(ILogger<EquFaultPhenomenonController> logger,
            IEquFaultPhenomenonService equFaultPhenomenonService)
        {
            _logger = logger;
            _equFaultPhenomenonService = equFaultPhenomenonService;
        }


        /// <summary>
        /// 添加（设备故障现象）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("设备故障现象", BusinessType.INSERT)]
        [PermissionDescription("equipment:equFaultPhenomenon:insert")]
        public async Task AddAsync(EquFaultPhenomenonSaveDto createDto)
        {
            await _equFaultPhenomenonService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（设备故障现象）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("设备故障现象", BusinessType.UPDATE)]
        [PermissionDescription("equipment:equFaultPhenomenon:update")]
        public async Task UpdateAsync(EquFaultPhenomenonSaveDto modifyDto)
        {
            await _equFaultPhenomenonService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（设备故障现象）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("设备故障现象", BusinessType.DELETE)]
        [PermissionDescription("equipment:equFaultPhenomenon:delete")]
        public async Task DeleteAsync(long[] ids)
        {
            await _equFaultPhenomenonService.DeletesAsync(ids);
        }

        /// <summary>
        /// 分页查询列表（设备故障现象）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
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
        public async Task<EquFaultPhenomenonDto?> QueryByIdAsync(long id)
        {
            return await _equFaultPhenomenonService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 查询ID集合（关联故障现象）
        /// </summary>
        /// <param name="phenomenonId"></param>
        /// <returns></returns>
        [HttpGet("reasons/{phenomenonId}")]
        public async Task<IEnumerable<long>> QueryReasonsByMainIdAsync(long phenomenonId)
        {
            return await _equFaultPhenomenonService.QueryReasonsByMainIdAsync(phenomenonId);
        }


        #region 状态变更
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("enable")]
        [PermissionDescription("equipment:equFaultPhenomenon:enable")]
        public async Task EnableAsync([FromBody] long id)
        {
            await _equFaultPhenomenonService.UpdateStatusAsync(new ChangeStatusDto
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
        [PermissionDescription("equipment:equFaultPhenomenon:retain")]
        public async Task RetainAsyn([FromBody] long id)
        {
            await _equFaultPhenomenonService.UpdateStatusAsync(new ChangeStatusDto
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
        [PermissionDescription("equipment:equFaultPhenomenon:abolish")]
        public async Task AbolishAsyn([FromBody] long id)
        {
            await _equFaultPhenomenonService.UpdateStatusAsync(new ChangeStatusDto
            {
                Id = id,
                Status = SysDataStatusEnum.Abolish
            });
        }
        #endregion

    }
}