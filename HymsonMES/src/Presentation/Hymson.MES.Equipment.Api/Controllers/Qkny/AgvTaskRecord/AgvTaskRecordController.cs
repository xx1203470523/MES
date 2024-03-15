using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.AgvTaskRecord;
using Hymson.MES.Services.Services.AgvTaskRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.AgvTaskRecord
{
    /// <summary>
    /// 控制器（AGV任务记录表）
    /// @author User
    /// @date 2024-03-11 11:37:12
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AgvTaskRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<AgvTaskRecordController> _logger;
        /// <summary>
        /// 服务接口（AGV任务记录表）
        /// </summary>
        private readonly IAgvTaskRecordService _agvTaskRecordService;


        /// <summary>
        /// 构造函数（AGV任务记录表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="agvTaskRecordService"></param>
        public AgvTaskRecordController(ILogger<AgvTaskRecordController> logger, IAgvTaskRecordService agvTaskRecordService)
        {
            _logger = logger;
            _agvTaskRecordService = agvTaskRecordService;
        }

        /// <summary>
        /// 添加（AGV任务记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] AgvTaskRecordSaveDto saveDto)
        {
             await _agvTaskRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（AGV任务记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] AgvTaskRecordSaveDto saveDto)
        {
             await _agvTaskRecordService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（AGV任务记录表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _agvTaskRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（AGV任务记录表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<AgvTaskRecordDto?> QueryByIdAsync(long id)
        {
            return await _agvTaskRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（AGV任务记录表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<AgvTaskRecordDto>> QueryPagedListAsync([FromQuery] AgvTaskRecordPagedQueryDto pagedQueryDto)
        {
            return await _agvTaskRecordService.GetPagedListAsync(pagedQueryDto);
        }

    }
}