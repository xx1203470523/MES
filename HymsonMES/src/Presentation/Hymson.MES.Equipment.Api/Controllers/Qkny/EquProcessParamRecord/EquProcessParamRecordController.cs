using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquProcessParamRecord;
using Hymson.MES.Services.Services.EquProcessParamRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.EquProcessParamRecord
{
    /// <summary>
    /// 控制器（过程参数记录表）
    /// @author Yxx
    /// @date 2024-03-11 04:44:02
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquProcessParamRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquProcessParamRecordController> _logger;
        /// <summary>
        /// 服务接口（过程参数记录表）
        /// </summary>
        private readonly IEquProcessParamRecordService _equProcessParamRecordService;


        /// <summary>
        /// 构造函数（过程参数记录表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equProcessParamRecordService"></param>
        public EquProcessParamRecordController(ILogger<EquProcessParamRecordController> logger, IEquProcessParamRecordService equProcessParamRecordService)
        {
            _logger = logger;
            _equProcessParamRecordService = equProcessParamRecordService;
        }

        /// <summary>
        /// 添加（过程参数记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] EquProcessParamRecordSaveDto saveDto)
        {
             await _equProcessParamRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（过程参数记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] EquProcessParamRecordSaveDto saveDto)
        {
             await _equProcessParamRecordService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（过程参数记录表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equProcessParamRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（过程参数记录表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquProcessParamRecordDto?> QueryByIdAsync(long id)
        {
            return await _equProcessParamRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（过程参数记录表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquProcessParamRecordDto>> QueryPagedListAsync([FromQuery] EquProcessParamRecordPagedQueryDto pagedQueryDto)
        {
            return await _equProcessParamRecordService.GetPagedListAsync(pagedQueryDto);
        }

    }
}