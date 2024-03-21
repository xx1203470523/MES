using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquToolLifeRecord;
using Hymson.MES.Services.Services.EquToolLifeRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.EquToolLifeRecord
{
    /// <summary>
    /// 控制器（设备夹具寿命）
    /// @author Yxx
    /// @date 2024-03-21 04:21:48
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquToolLifeRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquToolLifeRecordController> _logger;
        /// <summary>
        /// 服务接口（设备夹具寿命）
        /// </summary>
        private readonly IEquToolLifeRecordService _equToolLifeRecordService;


        /// <summary>
        /// 构造函数（设备夹具寿命）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equToolLifeRecordService"></param>
        public EquToolLifeRecordController(ILogger<EquToolLifeRecordController> logger, IEquToolLifeRecordService equToolLifeRecordService)
        {
            _logger = logger;
            _equToolLifeRecordService = equToolLifeRecordService;
        }

        /// <summary>
        /// 添加（设备夹具寿命）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] EquToolLifeRecordSaveDto saveDto)
        {
             await _equToolLifeRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备夹具寿命）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] EquToolLifeRecordSaveDto saveDto)
        {
             await _equToolLifeRecordService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备夹具寿命）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equToolLifeRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（设备夹具寿命）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquToolLifeRecordDto?> QueryByIdAsync(long id)
        {
            return await _equToolLifeRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（设备夹具寿命）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquToolLifeRecordDto>> QueryPagedListAsync([FromQuery] EquToolLifeRecordPagedQueryDto pagedQueryDto)
        {
            return await _equToolLifeRecordService.GetPagedListAsync(pagedQueryDto);
        }

    }
}