using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（产品检验参数组）
    /// @author Czhipu
    /// @date 2023-07-25 01:58:43
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcProductParameterGroupController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ProcProductParameterGroupController> _logger;
        /// <summary>
        /// 服务接口（产品检验参数组）
        /// </summary>
        private readonly IProcProductParameterGroupService _procProductParameterGroupService;


        /// <summary>
        /// 构造函数（产品检验参数组）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="procProductParameterGroupService"></param>
        public ProcProductParameterGroupController(ILogger<ProcProductParameterGroupController> logger, IProcProductParameterGroupService procProductParameterGroupService)
        {
            _logger = logger;
            _procProductParameterGroupService = procProductParameterGroupService;
        }

        /// <summary>
        /// 添加（产品检验参数组）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [PermissionDescription("process:procProductParameterGroup:insert")]
        public async Task AddAsync([FromBody] ProcProductParameterGroupSaveDto saveDto)
        {
            await _procProductParameterGroupService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（产品检验参数组）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [PermissionDescription("process:procProductParameterGroup:update")]
        public async Task UpdateAsync([FromBody] ProcProductParameterGroupSaveDto saveDto)
        {
            await _procProductParameterGroupService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（产品检验参数组）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [PermissionDescription("process:procProductParameterGroup:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _procProductParameterGroupService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（产品检验参数组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcProductParameterGroupInfoDto?> QueryByIdAsync(long id)
        {
            return await _procProductParameterGroupService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（产品检验参数表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("details/{id}")]
        public async Task<IEnumerable<ProcProductParameterGroupDetailDto>?> QueryDetailsByParameterGroupIdAsync(long id)
        {
            return await _procProductParameterGroupService.QueryDetailsByParameterGroupIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（产品检验参数组）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcProductParameterGroupDto>> QueryPagedListAsync([FromQuery] ProcProductParameterGroupPagedQueryDto pagedQueryDto)
        {
            return await _procProductParameterGroupService.GetPagedListAsync(pagedQueryDto);
        }

    }
}