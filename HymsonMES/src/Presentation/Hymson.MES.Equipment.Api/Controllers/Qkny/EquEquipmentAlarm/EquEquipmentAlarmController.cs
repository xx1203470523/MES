using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquEquipmentAlarm;
using Hymson.MES.Services.Services.EquEquipmentAlarm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.EquEquipmentAlarm
{
    /// <summary>
    /// 控制器（设备报警记录）
    /// @author Yxx
    /// @date 2024-03-08 09:11:19
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquEquipmentAlarmController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquEquipmentAlarmController> _logger;
        /// <summary>
        /// 服务接口（设备报警记录）
        /// </summary>
        private readonly IEquEquipmentAlarmService _equEquipmentAlarmService;


        /// <summary>
        /// 构造函数（设备报警记录）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equEquipmentAlarmService"></param>
        public EquEquipmentAlarmController(ILogger<EquEquipmentAlarmController> logger, IEquEquipmentAlarmService equEquipmentAlarmService)
        {
            _logger = logger;
            _equEquipmentAlarmService = equEquipmentAlarmService;
        }

        /// <summary>
        /// 添加（设备报警记录）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] EquEquipmentAlarmSaveDto saveDto)
        {
             await _equEquipmentAlarmService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备报警记录）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] EquEquipmentAlarmSaveDto saveDto)
        {
             await _equEquipmentAlarmService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备报警记录）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equEquipmentAlarmService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（设备报警记录）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquEquipmentAlarmDto?> QueryByIdAsync(long id)
        {
            return await _equEquipmentAlarmService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（设备报警记录）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquEquipmentAlarmDto>> QueryPagedListAsync([FromQuery] EquEquipmentAlarmPagedQueryDto pagedQueryDto)
        {
            return await _equEquipmentAlarmService.GetPagedListAsync(pagedQueryDto);
        }

    }
}