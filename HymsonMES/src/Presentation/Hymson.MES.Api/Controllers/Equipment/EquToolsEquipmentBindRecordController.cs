using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（工具绑定设备操作记录表）
    /// @author zhaoqing
    /// @date 2024-06-12 04:16:13
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquToolsEquipmentBindRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquToolsEquipmentBindRecordController> _logger;
        /// <summary>
        /// 服务接口（工具绑定设备操作记录表）
        /// </summary>
        private readonly IEquToolsEquipmentBindRecordService _equToolsEquipmentBindRecordService;


        /// <summary>
        /// 构造函数（工具绑定设备操作记录表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equToolsEquipmentBindRecordService"></param>
        public EquToolsEquipmentBindRecordController(ILogger<EquToolsEquipmentBindRecordController> logger, IEquToolsEquipmentBindRecordService equToolsEquipmentBindRecordService)
        {
            _logger = logger;
            _equToolsEquipmentBindRecordService = equToolsEquipmentBindRecordService;
        }

        /// <summary>
        /// 添加（工具绑定设备操作记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] EquToolsEquipmentBindRecordSaveDto saveDto)
        {
             await _equToolsEquipmentBindRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（工具绑定设备操作记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] EquToolsEquipmentBindRecordSaveDto saveDto)
        {
             await _equToolsEquipmentBindRecordService.ModifyAsync(saveDto);
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
            await _equToolsEquipmentBindRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（工具绑定设备操作记录表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquToolsEquipmentBindRecordDto?> QueryByIdAsync(long id)
        {
            return await _equToolsEquipmentBindRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（工具绑定设备操作记录表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquToolsEquipmentBindRecordDto>> QueryPagedListAsync([FromQuery] EquToolsEquipmentBindRecordPagedQueryDto pagedQueryDto)
        {
            return await _equToolsEquipmentBindRecordService.GetPagedListAsync(pagedQueryDto);
        }

    }
}