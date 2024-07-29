using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.EquSparepartRecord;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（工具绑定设备操作记录表）
    /// @author zhaoqing
    /// @date 2024-06-12 04:12:19
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquSparepartEquipmentBindRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquSparepartEquipmentBindRecordController> _logger;
        /// <summary>
        /// 服务接口（工具绑定设备操作记录表）
        /// </summary>
        private readonly IEquSparepartEquipmentBindRecordService _equSparepartEquipmentBindRecordService;

        /// <summary>
        /// 构造函数（工具绑定设备操作记录表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equSparepartEquipmentBindRecordService"></param>
        public EquSparepartEquipmentBindRecordController(ILogger<EquSparepartEquipmentBindRecordController> logger, IEquSparepartEquipmentBindRecordService equSparepartEquipmentBindRecordService)
        {
            _logger = logger;
            _equSparepartEquipmentBindRecordService = equSparepartEquipmentBindRecordService;
        }

        /// <summary>
        /// 安装（工具绑定设备操作记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("install")]
        public async Task InstallAsync([FromBody] EquSparepartEquipmentBindRecordCreateDto saveDto)
        {
             await _equSparepartEquipmentBindRecordService.InstallAsync(saveDto);
        }

        /// <summary>
        /// 卸载（工具绑定设备操作记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("uninstall")]
        public async Task UninstallAsync([FromBody] EquSparepartEquipmentBindRecordSaveDto saveDto)
        {
             await _equSparepartEquipmentBindRecordService.UninstallAsync(saveDto);
        }

        /// <summary>
        /// 删除（工具绑定设备操作记录表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equSparepartEquipmentBindRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（工具绑定设备操作记录表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquSparepartEquipmentBindRecordDto?> QueryByIdAsync(long id)
        {
            return await _equSparepartEquipmentBindRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（工具绑定设备操作记录表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquSparepartEquipmentBindRecordViewDto>> QueryPagedListAsync([FromQuery] EquSparepartEquipmentBindRecordPagedQueryDto pagedQueryDto)
        {
            return await _equSparepartEquipmentBindRecordService.GetPagedListAsync(pagedQueryDto);
        }

    }
}