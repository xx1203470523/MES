/*
 *describe: 工艺路线表    控制器 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:07:11
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.IProcessService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（工艺路线表）
    /// @author zhaoqing
    /// @date 2023-02-14 10:07:11
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcProcessRouteController : ControllerBase
    {
        /// <summary>
        /// 接口（工艺路线表）
        /// </summary>
        private readonly IProcProcessRouteService _procProcessRouteService;
        private readonly ILogger<ProcProcessRouteController> _logger;

        /// <summary>
        /// 构造函数（工艺路线表）
        /// </summary>
        /// <param name="procProcessRouteService"></param>
        public ProcProcessRouteController(IProcProcessRouteService procProcessRouteService, ILogger<ProcProcessRouteController> logger)
        {
            _procProcessRouteService = procProcessRouteService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（工艺路线表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<PagedInfo<ProcProcessRouteDto>> QueryPagedProcProcessRouteAsync([FromQuery] ProcProcessRoutePagedQueryDto parm)
        {
            return await _procProcessRouteService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（工艺路线表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<CustomProcessRouteDto> GetCustomProcProcessRouteAsync(long id)
        {
            return await _procProcessRouteService.GetCustomProcProcessRouteAsync(id);
        }

        /// <summary>
        /// 添加（工艺路线表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddProcProcessRouteAsync([FromBody] ProcProcessRouteCreateDto parm)
        {
             await _procProcessRouteService.AddProcProcessRouteAsync(parm);
        }

        /// <summary>
        /// 更新（工艺路线表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateProcProcessRouteAsync([FromBody] ProcProcessRouteModifyDto parm)
        {
            await _procProcessRouteService.UpdateProcProcessRouteAsync(parm);
        }

        /// <summary>
        /// 删除（工艺路线表）
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteProcProcessRouteAsync(DeleteDto deleteDto)
        {
            await _procProcessRouteService.DeleteProcProcessRouteAsync(deleteDto.Ids);
        }

    }
}