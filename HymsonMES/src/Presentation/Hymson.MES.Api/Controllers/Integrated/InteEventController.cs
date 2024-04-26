using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（事件维护）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteEventController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<InteEventController> _logger;
        /// <summary>
        /// 服务接口（事件维护）
        /// </summary>
        private readonly IInteEventService _inteEventService;


        /// <summary>
        /// 构造函数（事件维护）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteEventService"></param>
        public InteEventController(ILogger<InteEventController> logger, IInteEventService inteEventService)
        {
            _logger = logger;
            _inteEventService = inteEventService;
        }

        /// <summary>
        /// 添加（事件维护）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [PermissionDescription("integrated:inteEvent:insert")]
        [LogDescription("事件维护", BusinessType.INSERT)]
        public async Task<long> AddAsync([FromBody] InteEventSaveDto saveDto)
        {
            return await _inteEventService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（事件维护）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [PermissionDescription("integrated:inteEvent:update")]
        [LogDescription("事件维护", BusinessType.UPDATE)]
        public async Task UpdateAsync([FromBody] InteEventSaveDto saveDto)
        {
             await _inteEventService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（事件维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [PermissionDescription("integrated:inteEvent:delete")]
        [LogDescription("事件维护", BusinessType.DELETE)]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _inteEventService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（事件维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteEventInfoDto?> QueryByIdAsync(long id)
        {
            return await _inteEventService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（事件维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteEventDto>> QueryPagedListAsync([FromQuery] InteEventPagedQueryDto pagedQueryDto)
        {
            return await _inteEventService.GetPagedListAsync(pagedQueryDto);
        }

    }
}