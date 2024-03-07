using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquEquipmentHeartRecord;
using Hymson.MES.Services.Services.EquEquipmentHeartRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.EquEquipmentHeartRecord
{
    /// <summary>
    /// 控制器（设备心跳登录记录）
    /// @author Yxx
    /// @date 2024-03-07 03:39:54
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquEquipmentHeartRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquEquipmentHeartRecordController> _logger;
        /// <summary>
        /// 服务接口（设备心跳登录记录）
        /// </summary>
        private readonly IEquEquipmentHeartRecordService _equEquipmentHeartRecordService;


        /// <summary>
        /// 构造函数（设备心跳登录记录）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equEquipmentHeartRecordService"></param>
        public EquEquipmentHeartRecordController(ILogger<EquEquipmentHeartRecordController> logger, IEquEquipmentHeartRecordService equEquipmentHeartRecordService)
        {
            _logger = logger;
            _equEquipmentHeartRecordService = equEquipmentHeartRecordService;
        }

        /// <summary>
        /// 添加（设备心跳登录记录）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] EquEquipmentHeartRecordSaveDto saveDto)
        {
             await _equEquipmentHeartRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备心跳登录记录）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] EquEquipmentHeartRecordSaveDto saveDto)
        {
             await _equEquipmentHeartRecordService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备心跳登录记录）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equEquipmentHeartRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（设备心跳登录记录）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquEquipmentHeartRecordDto?> QueryByIdAsync(long id)
        {
            return await _equEquipmentHeartRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（设备心跳登录记录）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquEquipmentHeartRecordDto>> QueryPagedListAsync([FromQuery] EquEquipmentHeartRecordPagedQueryDto pagedQueryDto)
        {
            return await _equEquipmentHeartRecordService.GetPagedListAsync(pagedQueryDto);
        }

    }
}