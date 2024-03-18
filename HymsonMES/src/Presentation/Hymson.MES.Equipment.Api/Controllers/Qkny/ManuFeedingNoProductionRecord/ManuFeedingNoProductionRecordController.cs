using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.ManuFeedingNoProductionRecord;
using Hymson.MES.Services.Services.ManuFeedingNoProductionRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.ManuFeedingNoProductionRecord
{
    /// <summary>
    /// 控制器（设备投料非生产投料(洗罐子)）
    /// @author User
    /// @date 2024-03-18 11:51:44
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuFeedingNoProductionRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuFeedingNoProductionRecordController> _logger;
        /// <summary>
        /// 服务接口（设备投料非生产投料(洗罐子)）
        /// </summary>
        private readonly IManuFeedingNoProductionRecordService _manuFeedingNoProductionRecordService;


        /// <summary>
        /// 构造函数（设备投料非生产投料(洗罐子)）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuFeedingNoProductionRecordService"></param>
        public ManuFeedingNoProductionRecordController(ILogger<ManuFeedingNoProductionRecordController> logger, IManuFeedingNoProductionRecordService manuFeedingNoProductionRecordService)
        {
            _logger = logger;
            _manuFeedingNoProductionRecordService = manuFeedingNoProductionRecordService;
        }

        /// <summary>
        /// 添加（设备投料非生产投料(洗罐子)）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] ManuFeedingNoProductionRecordSaveDto saveDto)
        {
             await _manuFeedingNoProductionRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备投料非生产投料(洗罐子)）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] ManuFeedingNoProductionRecordSaveDto saveDto)
        {
             await _manuFeedingNoProductionRecordService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备投料非生产投料(洗罐子)）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _manuFeedingNoProductionRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（设备投料非生产投料(洗罐子)）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuFeedingNoProductionRecordDto?> QueryByIdAsync(long id)
        {
            return await _manuFeedingNoProductionRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（设备投料非生产投料(洗罐子)）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuFeedingNoProductionRecordDto>> QueryPagedListAsync([FromQuery] ManuFeedingNoProductionRecordPagedQueryDto pagedQueryDto)
        {
            return await _manuFeedingNoProductionRecordService.GetPagedListAsync(pagedQueryDto);
        }

    }
}