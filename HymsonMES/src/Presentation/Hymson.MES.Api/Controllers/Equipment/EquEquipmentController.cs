using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquEquipment;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMTC.EIS.Admin.WebApi.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备注册）
    /// @tableName equ_equipment
    /// @author Czhipu
    /// @date 2022-11-08
    /// </summary>

    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquEquipmentController : ControllerBase
    {
        /// <summary>
        /// 接口（设备注册）
        /// </summary>
        private readonly IEquEquipmentService _equEquipmentService;
        private readonly ILogger<EquEquipmentController> _logger;

        /// <summary>
        /// 构造函数（设备注册）
        /// </summary>
        /// <param name="equEquipmentService"></param>
        /// <param name="logger"></param>
        public EquEquipmentController(IEquEquipmentService equEquipmentService, ILogger<EquEquipmentController> logger)
        {
            _equEquipmentService = equEquipmentService;
            _logger = logger;
        }

        /// <summary>
        /// 添加（设备注册）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("设备注册", BusinessType.INSERT)]
        [PermissionDescription("equ:equipment:insert")]
        public async Task CreateAsync([FromBody]EquEquipmentSaveDto createDto)
        {
            await _equEquipmentService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（设备注册）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("设备注册", BusinessType.UPDATE)]
        [PermissionDescription("equ:equipment:update")]
        public async Task ModifyAsync(EquEquipmentSaveDto modifyDto)
        {
            await _equEquipmentService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（设备注册）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("设备注册", BusinessType.DELETE)]
        [PermissionDescription("equ:equipment:delete")]
        public async Task DeletesAsync(long[] ids)
        {
            await _equEquipmentService.DeletesAsync(ids);
        }

        /// <summary>
        /// 分页查询列表（设备注册）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [Route("page")]
        [HttpGet]
        //[PermissionDescription("equ:equipment:list")]
        public async Task<PagedInfo<EquEquipmentListDto>> GetPageListAsync([FromQuery] EquEquipmentPagedQueryDto pagedQueryDto)
        {
            return await _equEquipmentService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（设备注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquEquipmentDto> GetDetailAsync(long id)
        {
            return await _equEquipmentService.GetDetailAsync(id);
        }

        /// <summary>
        /// 查询字典（设备注册）
        /// </summary>
        /// <returns></returns>
        [HttpGet("dictionary")]
        public async Task<IEnumerable<EquEquipmentDictionaryDto>> QueryEquEquipmentDictionaryAsync()
        {
            return await _equEquipmentService.GetEquEquipmentDictionaryAsync();
        }

        /// <summary>
        ///  获取设备关联硬件数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("linkHardware/list")]
        public async Task<PagedInfo<EquEquipmentLinkHardwareBaseDto>> GetEquipmentLinkHardwareAsync(EquEquipmentLinkHardwarePagedQueryDto pagedQueryDto)
        {
            return await _equEquipmentService.GetEquimentLinkHardwareAsync(pagedQueryDto);
        }

        /// <summary>
        ///  获取设备关联Api数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("linkApi/list")]
        public async Task<PagedInfo<EquEquipmentLinkApiBaseDto>> GetEquipmentLinkApiAsync(EquEquipmentLinkApiPagedQueryDto pagedQueryDto)
        {
            return await _equEquipmentService.GetEquimentLinkApiAsync(pagedQueryDto);
        }

        /// <summary>
        /// 获取设备Token
        /// </summary>
        /// <param name="EquEquipmentId"></param>
        /// <returns></returns>
        [Route("token/{equEquipmentId}")]
        [PermissionDescription("equ:equipment:token")]
        [HttpGet]
        public async Task<string> GetEquEquipmentTokenAsync(long EquEquipmentId) 
        {
            return await _equEquipmentService.GetEquEquipmentTokenAsync(EquEquipmentId);
        }
    }
}