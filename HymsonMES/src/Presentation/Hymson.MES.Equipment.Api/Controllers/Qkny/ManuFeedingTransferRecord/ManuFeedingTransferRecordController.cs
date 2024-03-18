using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.ManuFeedingTransferRecord;
using Hymson.MES.Services.Services.ManuFeedingTransferRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.ManuFeedingTransferRecord
{
    /// <summary>
    /// 控制器（上料信息转移记录）
    /// @author Yxx
    /// @date 2024-03-18 11:19:42
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuFeedingTransferRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuFeedingTransferRecordController> _logger;
        /// <summary>
        /// 服务接口（上料信息转移记录）
        /// </summary>
        private readonly IManuFeedingTransferRecordService _manuFeedingTransferRecordService;


        /// <summary>
        /// 构造函数（上料信息转移记录）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuFeedingTransferRecordService"></param>
        public ManuFeedingTransferRecordController(ILogger<ManuFeedingTransferRecordController> logger, IManuFeedingTransferRecordService manuFeedingTransferRecordService)
        {
            _logger = logger;
            _manuFeedingTransferRecordService = manuFeedingTransferRecordService;
        }

        /// <summary>
        /// 添加（上料信息转移记录）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] ManuFeedingTransferRecordSaveDto saveDto)
        {
             await _manuFeedingTransferRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（上料信息转移记录）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] ManuFeedingTransferRecordSaveDto saveDto)
        {
             await _manuFeedingTransferRecordService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（上料信息转移记录）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _manuFeedingTransferRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（上料信息转移记录）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuFeedingTransferRecordDto?> QueryByIdAsync(long id)
        {
            return await _manuFeedingTransferRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（上料信息转移记录）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuFeedingTransferRecordDto>> QueryPagedListAsync([FromQuery] ManuFeedingTransferRecordPagedQueryDto pagedQueryDto)
        {
            return await _manuFeedingTransferRecordService.GetPagedListAsync(pagedQueryDto);
        }

    }
}