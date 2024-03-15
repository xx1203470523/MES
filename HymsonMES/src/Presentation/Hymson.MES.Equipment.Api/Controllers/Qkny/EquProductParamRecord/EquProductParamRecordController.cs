using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquProductParamRecord;
using Hymson.MES.Services.Services.EquProductParamRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.EquProductParamRecord
{
    /// <summary>
    /// 控制器（产品参数记录表）
    /// @author Yxx
    /// @date 2024-03-13 04:43:35
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquProductParamRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquProductParamRecordController> _logger;
        /// <summary>
        /// 服务接口（产品参数记录表）
        /// </summary>
        private readonly IEquProductParamRecordService _equProductParamRecordService;


        /// <summary>
        /// 构造函数（产品参数记录表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equProductParamRecordService"></param>
        public EquProductParamRecordController(ILogger<EquProductParamRecordController> logger, IEquProductParamRecordService equProductParamRecordService)
        {
            _logger = logger;
            _equProductParamRecordService = equProductParamRecordService;
        }

        /// <summary>
        /// 添加（产品参数记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] EquProductParamRecordSaveDto saveDto)
        {
             await _equProductParamRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（产品参数记录表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] EquProductParamRecordSaveDto saveDto)
        {
             await _equProductParamRecordService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（产品参数记录表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equProductParamRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（产品参数记录表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquProductParamRecordDto?> QueryByIdAsync(long id)
        {
            return await _equProductParamRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（产品参数记录表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquProductParamRecordDto>> QueryPagedListAsync([FromQuery] EquProductParamRecordPagedQueryDto pagedQueryDto)
        {
            return await _equProductParamRecordService.GetPagedListAsync(pagedQueryDto);
        }

    }
}