using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment;
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
        /// 获取分页数据
        /// </summary>
        /// <param name="equipmentUnitPagedQueryDto"></param>
        /// <returns></returns>
        [Route("pagelist")]
        [HttpGet]
        public async Task<PagedInfo<EquipmentUnitDto>> GetList([FromQuery] EquipmentUnitPagedQueryDto equipmentUnitPagedQueryDto)
        {
            return await _equipmentUnitService.GetListAsync(equipmentUnitPagedQueryDto);
        }

        /// <summary>
        /// 新增（单位）
        /// </summary>
        /// <param name="equipmentUnitDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task Create(EquipmentUnitDto equipmentUnitDto)
        {
            await _equipmentUnitService.CreateEquipmentUnitAsync(equipmentUnitDto);
        }

        /// <summary>
        /// 更新（单位）
        /// </summary>
        /// <param name="equipmentUnitDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task Update(EquipmentUnitDto equipmentUnitDto)
        {
            await _equipmentUnitService.ModifyEquipmentUnitAsync(equipmentUnitDto);
        }

        /// <summary>
        /// 删除（单位）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task Delete(long id)
        {
            await _equipmentUnitService.DeleteEquipmentUnitAsync(id);
        }
    }
}