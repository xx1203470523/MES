/*
 *creator: Karl
 *
 *describe: 标准参数表    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-13 02:50:20
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（标准参数表）
    /// @author Karl
    /// @date 2023-02-13 02:50:20
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcParameterController : ControllerBase
    {
        /// <summary>
        /// 接口（标准参数表）
        /// </summary>
        private readonly IProcParameterService _procParameterService;
        private readonly ILogger<ProcParameterController> _logger;

        /// <summary>
        /// 构造函数（标准参数表）
        /// </summary>
        /// <param name="procParameterService"></param>
        public ProcParameterController(IProcParameterService procParameterService, ILogger<ProcParameterController> logger)
        {
            _procParameterService = procParameterService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（标准参数表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcParameterDto>> QueryPagedProcParameterAsync([FromQuery] ProcParameterPagedQueryDto parm)
        {
            return await _procParameterService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（标准参数表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcParameterDto> QueryProcParameterByIdAsync(long id)
        {
            return await _procParameterService.QueryProcParameterByIdAsync(id);
        }

        /// <summary>
        /// 添加（标准参数表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddProcParameterAsync([FromBody] ProcParameterCreateDto parm)
        {
             await _procParameterService.CreateProcParameterAsync(parm);
        }

        /// <summary>
        /// 更新（标准参数表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task UpdateProcParameterAsync([FromBody] ProcParameterModifyDto parm)
        {
             await _procParameterService.ModifyProcParameterAsync(parm);
        }

        /// <summary>
        /// 删除（标准参数表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task DeleteProcParameterAsync(string ids)
        {
            //long[] idsArr = StringExtension.SpitLongArrary(ids);
            await _procParameterService.DeletesProcParameterAsync(ids);
        }

    }
}