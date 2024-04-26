using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（消息组）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteMessageGroupController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<InteMessageGroupController> _logger;
        /// <summary>
        /// 服务接口（消息组）
        /// </summary>
        private readonly IInteMessageGroupService _inteMessageGroupService;


        /// <summary>
        /// 构造函数（消息组）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteMessageGroupService"></param>
        public InteMessageGroupController(ILogger<InteMessageGroupController> logger, IInteMessageGroupService inteMessageGroupService)
        {
            _logger = logger;
            _inteMessageGroupService = inteMessageGroupService;
        }

        /// <summary>
        /// 添加（消息组）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("消息组", BusinessType.INSERT)]
        [PermissionDescription("integrated:inteMessageGroup:insert")]
        public async Task<long> AddAsync([FromBody] InteMessageGroupSaveDto saveDto)
        {
            return await _inteMessageGroupService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（消息组）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]       
        [Route("update")]
        [LogDescription("消息组", BusinessType.UPDATE)]
        [PermissionDescription("integrated:inteMessageGroup:update")]
        public async Task UpdateAsync([FromBody] InteMessageGroupSaveDto saveDto)
        {
             await _inteMessageGroupService.ModifyAsync(saveDto);
        }
        
        /// <summary>
        /// 删除（消息组）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("消息组", BusinessType.DELETE)]
        [PermissionDescription("integrated:inteMessageGroup:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _inteMessageGroupService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（消息组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteMessageGroupDto?> QueryByIdAsync(long id)
        {
            return await _inteMessageGroupService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（消息组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("details/{id}")]
        public async Task<IEnumerable<InteMessageGroupPushMethodDto>?> QueryDetailsByMainIdAsync(long id)
        {
            return await _inteMessageGroupService.QueryDetailsByMainIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（消息组）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteMessageGroupDto>> QueryPagedListAsync([FromQuery] InteMessageGroupPagedQueryDto pagedQueryDto)
        {
            return await _inteMessageGroupService.GetPagedListAsync(pagedQueryDto);
        }

    }
}