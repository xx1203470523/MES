using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquEquipmentUnit;
using Hymson.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（单位维护）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquEquipmentUnitController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IEquEquipmentUnitService _equipmentUnitService;
        private readonly ILogger<EquEquipmentUnitController> _logger;

        /// <summary>
        /// 构造函数（单位维护）
        /// </summary>
        /// <param name="equipmentUnitService"></param>
        /// <param name="logger"></param>
        public EquEquipmentUnitController(IEquEquipmentUnitService equipmentUnitService, ILogger<EquEquipmentUnitController> logger)
        {
            _equipmentUnitService = equipmentUnitService;
            _logger = logger;
        }

        /// <summary>
        /// 新增（单位维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<int> Create(EquEquipmentUnitCreateDto createDto)
        {
            return await _equipmentUnitService.CreateEquipmentUnitAsync(createDto);
        }

        /// <summary>
        /// 更新（单位维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<int> Modify(EquEquipmentUnitModifyDto modifyDto)
        {
            return await _equipmentUnitService.ModifyEquipmentUnitAsync(modifyDto);
        }

        /// <summary>
        /// 删除（单位维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<int> Delete(string ids)
        {
            long[] idsArr = StringExtension.SpitLongArrary(ids);
            return await _equipmentUnitService.DeleteEquipmentUnitAsync(idsArr);
        }

        /// <summary>
        /// 获取分页数据（单位维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [Route("pagelist")]
        [HttpGet]
        public async Task<PagedInfo<EquEquipmentUnitDto>> GetPagedListAsync([FromQuery] EquEquipmentUnitPagedQueryDto pagedQueryDto)
        {
            return await _equipmentUnitService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（单位维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquEquipmentUnitDto> GetEntityAsync(long id)
        {
            return await _equipmentUnitService.GetEntityAsync(id);
        }
    }
}