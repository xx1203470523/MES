using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.ManuFeedingCompletedZjyjRecord;
using Hymson.MES.Services.Services.ManuFeedingCompletedZjyjRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.ManuFeedingCompletedZjyjRecord
{
    /// <summary>
    /// 控制器（manu_feeding_completed_zjyj_record）
    /// @author Yxx
    /// @date 2024-03-15 11:04:42
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuFeedingCompletedZjyjRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuFeedingCompletedZjyjRecordController> _logger;
        /// <summary>
        /// 服务接口（manu_feeding_completed_zjyj_record）
        /// </summary>
        private readonly IManuFeedingCompletedZjyjRecordService _manuFeedingCompletedZjyjRecordService;


        /// <summary>
        /// 构造函数（manu_feeding_completed_zjyj_record）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuFeedingCompletedZjyjRecordService"></param>
        public ManuFeedingCompletedZjyjRecordController(ILogger<ManuFeedingCompletedZjyjRecordController> logger, IManuFeedingCompletedZjyjRecordService manuFeedingCompletedZjyjRecordService)
        {
            _logger = logger;
            _manuFeedingCompletedZjyjRecordService = manuFeedingCompletedZjyjRecordService;
        }

        /// <summary>
        /// 添加（manu_feeding_completed_zjyj_record）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] ManuFeedingCompletedZjyjRecordSaveDto saveDto)
        {
             await _manuFeedingCompletedZjyjRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（manu_feeding_completed_zjyj_record）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] ManuFeedingCompletedZjyjRecordSaveDto saveDto)
        {
             await _manuFeedingCompletedZjyjRecordService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（manu_feeding_completed_zjyj_record）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _manuFeedingCompletedZjyjRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（manu_feeding_completed_zjyj_record）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuFeedingCompletedZjyjRecordDto?> QueryByIdAsync(long id)
        {
            return await _manuFeedingCompletedZjyjRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（manu_feeding_completed_zjyj_record）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuFeedingCompletedZjyjRecordDto>> QueryPagedListAsync([FromQuery] ManuFeedingCompletedZjyjRecordPagedQueryDto pagedQueryDto)
        {
            return await _manuFeedingCompletedZjyjRecordService.GetPagedListAsync(pagedQueryDto);
        }

    }
}