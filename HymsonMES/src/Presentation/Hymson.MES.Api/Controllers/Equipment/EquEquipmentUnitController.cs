using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquEquipmentUnit;
using Hymson.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// ����������λά����
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
        /// ���캯������λά����
        /// </summary>
        /// <param name="equipmentUnitService"></param>
        /// <param name="logger"></param>
        public EquEquipmentUnitController(IEquEquipmentUnitService equipmentUnitService, ILogger<EquEquipmentUnitController> logger)
        {
            _equipmentUnitService = equipmentUnitService;
            _logger = logger;
        }

        /// <summary>
        /// ��ӣ���λά����
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task CreateAsync(EquEquipmentUnitSaveDto createDto)
        {
            await _equipmentUnitService.CreateAsync(createDto);
        }

        /// <summary>
        /// ���£���λά����
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task ModifyAsync(EquEquipmentUnitSaveDto modifyDto)
        {
            await _equipmentUnitService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// ɾ������λά����
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeletesAsync(long[] ids)
        {
            await _equipmentUnitService.DeletesAsync(ids);
        }

        /// <summary>
        /// ��ȡ��ҳ���ݣ���λά����
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [Route("page")]
        [HttpGet]
        public async Task<PagedInfo<EquEquipmentUnitDto>> GetPagedListAsync([FromQuery] EquEquipmentUnitPagedQueryDto pagedQueryDto)
        {
            return await _equipmentUnitService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// ��ѯ���飨��λά����
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