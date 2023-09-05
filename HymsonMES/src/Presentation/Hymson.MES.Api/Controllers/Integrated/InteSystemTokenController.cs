/*
 *creator: Karl
 *
 *describe: 系统Token    控制器 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-06-15 02:09:57
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（系统Token）
    /// @author zhaoqing
    /// @date 2023-06-15 02:09:57
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteSystemTokenController : ControllerBase
    {
        /// <summary>
        /// 接口（系统Token）
        /// </summary>
        private readonly IInteSystemTokenService _inteSystemTokenService;
        private readonly ILogger<InteSystemTokenController> _logger;

        /// <summary>
        /// 构造函数（系统Token）
        /// </summary>
        /// <param name="inteSystemTokenService"></param>
        /// <param name="logger"></param>
        public InteSystemTokenController(IInteSystemTokenService inteSystemTokenService, ILogger<InteSystemTokenController> logger)
        {
            _inteSystemTokenService = inteSystemTokenService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（系统Token）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteSystemTokenDto>> QueryPagedInteSystemTokenAsync([FromQuery] InteSystemTokenPagedQueryDto parm)
        {
            return await _inteSystemTokenService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（系统Token）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteSystemTokenDto> QueryInteSystemTokenByIdAsync(long id)
        {
            return await _inteSystemTokenService.QueryInteSystemTokenByIdAsync(id);
        }

        /// <summary>
        /// 添加（系统Token）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        [LogDescription("系统Token", BusinessType.INSERT)]
        [PermissionDescription("inte:systemToken:insert")]
        public async Task AddInteSystemTokenAsync([FromBody] InteSystemTokenCreateDto parm)
        {
            await _inteSystemTokenService.CreateInteSystemTokenAsync(parm);
        }

        /// <summary>
        /// 更新（系统Token）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("系统Token", BusinessType.UPDATE)]
        [PermissionDescription("inte:systemToken:update")]
        public async Task UpdateInteSystemTokenAsync([FromBody] InteSystemTokenModifyDto parm)
        {
            await _inteSystemTokenService.ModifyInteSystemTokenAsync(parm);
        }

        /// <summary>
        /// 删除（系统Token）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("系统Token", BusinessType.DELETE)]
        [PermissionDescription("inte:systemToken:delete")]
        public async Task DeleteInteSystemTokenAsync([FromBody] long[] ids)
        {
            await _inteSystemTokenService.DeletesInteSystemTokenAsync(ids);
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refreshToken/{systemId}")]
        [PermissionDescription("inte:systemToken:refreshToken")]
        public async Task<string> RefreshSystemTokenAsync(long systemId)
        {
            return await _inteSystemTokenService.RefreshSystemTokenAsync(systemId);
        }
    }
}