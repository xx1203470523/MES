using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.ManuEquipmentStatusTime;
using Hymson.MES.Services.Services.ManuEquipmentStatusTime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.ManuEquipmentStatusTime
{
    /// <summary>
    /// 控制器（设备状态时间）
    /// @author Yxx
    /// @date 2024-03-07 07:32:15
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuEquipmentStatusTimeController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuEquipmentStatusTimeController> _logger;
        /// <summary>
        /// 服务接口（设备状态时间）
        /// </summary>
        private readonly IManuEquipmentStatusTimeService _manuEquipmentStatusTimeService;


        /// <summary>
        /// 构造函数（设备状态时间）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuEquipmentStatusTimeService"></param>
        public ManuEquipmentStatusTimeController(ILogger<ManuEquipmentStatusTimeController> logger, IManuEquipmentStatusTimeService manuEquipmentStatusTimeService)
        {
            _logger = logger;
            _manuEquipmentStatusTimeService = manuEquipmentStatusTimeService;
        }

        /// <summary>
        /// 添加（设备状态时间）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] ManuEquipmentStatusTimeSaveDto saveDto)
        {
             await _manuEquipmentStatusTimeService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备状态时间）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] ManuEquipmentStatusTimeSaveDto saveDto)
        {
             await _manuEquipmentStatusTimeService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备状态时间）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _manuEquipmentStatusTimeService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（设备状态时间）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuEquipmentStatusTimeDto?> QueryByIdAsync(long id)
        {
            return await _manuEquipmentStatusTimeService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（设备状态时间）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuEquipmentStatusTimeDto>> QueryPagedListAsync([FromQuery] ManuEquipmentStatusTimePagedQueryDto pagedQueryDto)
        {
            return await _manuEquipmentStatusTimeService.GetPagedListAsync(pagedQueryDto);
        }

    }
}