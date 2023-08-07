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
    /// @author Czhipu
    /// @date 2023-08-03 02:42:52
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteEventTypeController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<InteEventTypeController> _logger;
        /// <summary>
        /// 服务接口（事件维护）
        /// </summary>
        private readonly IInteEventTypeService _inteEventTypeService;


        /// <summary>
        /// 构造函数（事件维护）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteEventTypeService"></param>
        public InteEventTypeController(ILogger<InteEventTypeController> logger, IInteEventTypeService inteEventTypeService)
        {
            _logger = logger;
            _inteEventTypeService = inteEventTypeService;
        }

        /// <summary>
        /// 添加（事件维护）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [PermissionDescription("integrated:inteEventType:insert")]
        public async Task AddAsync([FromBody] InteEventTypeSaveDto saveDto)
        {
            await _inteEventTypeService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（事件维护）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [PermissionDescription("integrated:inteEventType:update")]
        public async Task UpdateAsync([FromBody] InteEventTypeSaveDto saveDto)
        {
            await _inteEventTypeService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（事件维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [PermissionDescription("integrated:inteEventType:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _inteEventTypeService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（事件维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteEventTypeDto?> QueryByIdAsync(long id)
        {
            return await _inteEventTypeService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（关联群组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("messageGroups/{id}")]
        public async Task<IEnumerable<InteEventTypeMessageGroupRelationDto>?> QueryMessageGroupsByMainIdAsync(long id)
        {
            return await _inteEventTypeService.QueryMessageGroupsByMainIdAsync(id);
        }

        // TODO 读取接收升级数据

        // TODO 读取处理升级数据

        /// <summary>
        /// 查询详情（推送规则）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("rules/{id}")]
        public async Task<IEnumerable<InteEventTypePushRuleDto>> QueryRulesByMainIdAsync(long id)
        {
            return await _inteEventTypeService.QueryRulesByMainIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（事件维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteEventTypeDto>> QueryPagedListAsync([FromQuery] InteEventTypePagedQueryDto pagedQueryDto)
        {
            return await _inteEventTypeService.GetPagedListAsync(pagedQueryDto);
        }

    }
}