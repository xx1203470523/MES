using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquEquipmentUnit;
using Hymson.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（单位维护）
    /// </summary>
    [Authorize]
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
        /// 添加（单位维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<int> CreateAsync(EquEquipmentUnitCreateDto createDto)
        {
            return await _equipmentUnitService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（单位维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<int> ModifyAsync(EquEquipmentUnitModifyDto modifyDto)
        {
            return await _equipmentUnitService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（单位维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<int> DeletesAsync(string ids)
        {
            long[] idsArr = StringExtension.SpitLongArrary(ids);
            return await _equipmentUnitService.DeletesAsync(idsArr);
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
        public async Task<EquEquipmentUnitDto> GetDetailAsync(long id)
        {
            return await _equipmentUnitService.GetDetailAsync(id);
        }
    }
}