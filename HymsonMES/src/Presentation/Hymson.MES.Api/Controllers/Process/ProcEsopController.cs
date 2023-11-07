/*
 *creator: Karl
 *
 *describe: ESOP    控制器 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-11-02 02:39:53
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（ESOP）
    /// @author zhaoqing
    /// @date 2023-11-02 02:39:53
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcEsopController : ControllerBase
    {
        /// <summary>
        /// 接口（ESOP）
        /// </summary>
        private readonly IProcEsopService _procEsopService;
        private readonly ILogger<ProcEsopController> _logger;

        /// <summary>
        /// 构造函数（ESOP）
        /// </summary>
        /// <param name="procEsopService"></param>
        public ProcEsopController(IProcEsopService procEsopService, ILogger<ProcEsopController> logger)
        {
            _procEsopService = procEsopService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（ESOP）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcEsopDto>> QueryPagedProcEsopAsync([FromQuery] ProcEsopPagedQueryDto parm)
        {
            return await _procEsopService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（ESOP）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcEsopDto> QueryProcEsopByIdAsync(long id)
        {
            return await _procEsopService.QueryProcEsopByIdAsync(id);
        }

        /// <summary>
        /// 添加（ESOP）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [PermissionDescription("proc:esop:insert")]
        public async Task AddProcEsopAsync([FromBody] ProcEsopCreateDto parm)
        {
            await _procEsopService.CreateProcEsopAsync(parm);
        }

        /// <summary>
        /// 更新（ESOP）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [PermissionDescription("proc:esop:update")]
        public async Task UpdateProcEsopAsync([FromBody] ProcEsopModifyDto parm)
        {
            await _procEsopService.ModifyProcEsopAsync(parm);
        }

        /// <summary>
        /// 删除（ESOP）
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [PermissionDescription("proc:esop:delete")]
        public async Task DeleteProcEsopAsync(DeleteDto deleteDto)
        {
            await _procEsopService.DeletesProcEsopAsync(deleteDto.Ids);
        }

        #endregion
    }
}