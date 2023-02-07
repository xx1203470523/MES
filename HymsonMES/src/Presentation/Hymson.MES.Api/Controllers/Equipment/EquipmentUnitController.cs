using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment.EquipmentUnit;
using Hymson.MES.Services.Services.Equipment.EquipmentUnit;
using Hymson.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquipmentUnitController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IEquipmentUnitService _equipmentUnitService;
        private readonly ILogger<EquipmentUnitController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitService"></param>
        /// <param name="logger"></param>
        public EquipmentUnitController(IEquipmentUnitService equipmentUnitService, ILogger<EquipmentUnitController> logger)
        {
            _equipmentUnitService = equipmentUnitService;
            _logger = logger;
        }

        /// <summary>
        /// 新增（单位）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<int> Create(EquipmentUnitCreateDto createDto)
        {
            return await _equipmentUnitService.CreateEquipmentUnitAsync(createDto);
        }

        /// <summary>
        /// 更新（单位）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<int> Modify(EquipmentUnitModifyDto modifyDto)
        {
            return await _equipmentUnitService.ModifyEquipmentUnitAsync(modifyDto);
        }

        /// <summary>
        /// 删除（单位）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task<int> Delete(string ids)
        {
            long[] idsArr = StringExtension.SpitLongArrary(ids);
            return await _equipmentUnitService.DeleteEquipmentUnitAsync(idsArr);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [Route("pagelist")]
        [HttpGet]
        public async Task<PagedInfo<EquipmentUnitDto>> GetList([FromQuery] EquipmentUnitPagedQueryDto pagedQueryDto)
        {
            return await _equipmentUnitService.GetListAsync(pagedQueryDto);
        }

    }
}