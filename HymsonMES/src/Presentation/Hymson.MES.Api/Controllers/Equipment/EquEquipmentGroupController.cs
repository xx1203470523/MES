using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.EquEquipmentGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备组）
    /// @author 陈志谱
    /// @date 2023-02-08 02:43:18
    /// </summary>
    //
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquEquipmentGroupController : ControllerBase
    {
        /// <summary>
        /// 接口（设备组）
        /// </summary>
        private readonly IEquEquipmentGroupService _equEquipmentGroupService;
        private readonly ILogger<EquEquipmentGroupController> _logger;

        /// <summary>
        /// 构造函数（设备组）
        /// </summary>
        /// <param name="equEquipmentGroupService"></param>
        public EquEquipmentGroupController(IEquEquipmentGroupService equEquipmentGroupService, ILogger<EquEquipmentGroupController> logger)
        {
            _equEquipmentGroupService = equEquipmentGroupService;
            _logger = logger;
        }

        /// <summary>
        /// 添加（设备组）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task CreateAsync(EquEquipmentGroupSaveDto createDto)
        {
            await _equEquipmentGroupService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（设备组）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task ModifyAsync(EquEquipmentGroupSaveDto modifyDto)
        {
            await _equEquipmentGroupService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（设备组）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeletesAsync(long[] ids)
        {
            await _equEquipmentGroupService.DeletesAsync(ids);
        }

        /// <summary>
        /// 分页查询列表（设备组）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        public async Task<PagedInfo<EquEquipmentGroupListDto>> GetPagedListAsync([FromQuery] EquEquipmentGroupPagedQueryDto pagedQueryDto)
        {
            return await _equEquipmentGroupService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（设备组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquEquipmentGroupDto> GetDetailAsync(long id)
        {
            return await _equEquipmentGroupService.GetDetailAsync(id);
        }
    }
}