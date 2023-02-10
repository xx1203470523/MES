using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquipmentUnit;
using Hymson.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// ����������λά����
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
        /// ���캯������λά����
        /// </summary>
        /// <param name="equipmentUnitService"></param>
        /// <param name="logger"></param>
        public EquipmentUnitController(IEquipmentUnitService equipmentUnitService, ILogger<EquipmentUnitController> logger)
        {
            _equipmentUnitService = equipmentUnitService;
            _logger = logger;
        }

        /// <summary>
        /// ��������λά����
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
        /// ���£���λά����
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
        /// ɾ������λά����
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
        /// ��ȡ��ҳ���ݣ���λά����
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