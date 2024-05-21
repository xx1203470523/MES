using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquEquipmentFaultType;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备故障类型）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquEquipmentFaultTypeController : ControllerBase
    {
        private readonly IEquEquipmentFaultTypeService _equEquipmentFaultTypeService;
        private readonly ILogger<EquEquipmentFaultTypeController> _logger;

        /// <summary>
        /// 构造函数（设备故障类型）
        /// </summary>
        /// <param name="equEquipmentFaultTypeService"></param>
        /// <param name="logger"></param>
        public EquEquipmentFaultTypeController(IEquEquipmentFaultTypeService equEquipmentFaultTypeService, ILogger<EquEquipmentFaultTypeController> logger)
        {
            _equEquipmentFaultTypeService = equEquipmentFaultTypeService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（设备故障类型）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquipmentFaultTypeDto>> QueryPagedEquipmentFaultTypeAsync([FromQuery] EquipmentFaultTypePagedQueryDto parm)
        {
            return await _equEquipmentFaultTypeService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（通过ID查询设备故障类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquipmentFaultTypeDto> QueryEquipmentFaultTypeByIdAsync(long id)
        {
            return await _equEquipmentFaultTypeService.QueryQualUnqualifiedGroupByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（设备故障类型关联设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/unqualifiedCodeList")]
        public async Task<List<EquipmentFaultTypePhenomenonRelationDto>> QueryQualUnqualifiedCodeGroupRelationByIdAsync(long id)
        {
            return await _equEquipmentFaultTypeService.GetQualUnqualifiedCodeGroupRelationByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（设备故障类型关联设备组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/procedureList")]
        public async Task<List<EquipmentFaultTypeEquipmentGroupRelationDto>> QueryQualUnqualifiedCodeProcedureRelationpByIdAsync(long id)
        {
            return await _equEquipmentFaultTypeService.GetQualUnqualifiedCodeProcedureRelationByIdAsync(id);
        }

        /// <summary>
        /// 添加（设备故障类型）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("设备故障类型", BusinessType.INSERT)]
        [PermissionDescription("qual:unqualifiedGroup:insert")]
        public async Task<long> AddQualUnqualifiedGroupAsync([FromBody] EQualUnqualifiedGroupCreateDto parm)
        {
            return await _equEquipmentFaultTypeService.CreateQualUnqualifiedGroupAsync(parm);
        }
   
        /// <summary>
        /// 更新（设备故障类型）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("设备故障类型", BusinessType.UPDATE)]
        [PermissionDescription("qual:unqualifiedGroup:update")]
        public async Task UpdateQualUnqualifiedGroupAsync([FromBody] EQualUnqualifiedGroupModifyDto parm)
        {
            await _equEquipmentFaultTypeService.ModifyQualUnqualifiedGroupAsync(parm);
        }

        /// <summary>
        /// 删除（设备故障类型）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("设备故障类型", BusinessType.DELETE)]
        [PermissionDescription("qual:unqualifiedGroup:delete")]
        public async Task DeleteQualUnqualifiedGroupAsync(long[] ids)
        {
            await _equEquipmentFaultTypeService.DeletesQualUnqualifiedGroupAsync(ids);
        }
    }
}