/*
 *creator: Karl
 *
 *describe: 上料点表    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（上料点表）
    /// @author Karl
    /// @date 2023-02-17 08:57:53
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcLoadPointController : ControllerBase
    {
        /// <summary>
        /// 接口（上料点表）
        /// </summary>
        private readonly IProcLoadPointService _procLoadPointService;
        private readonly ILogger<ProcLoadPointController> _logger;

        /// <summary>
        /// 构造函数（上料点表）
        /// </summary>
        /// <param name="procLoadPointService"></param>
        public ProcLoadPointController(IProcLoadPointService procLoadPointService, ILogger<ProcLoadPointController> logger)
        {
            _procLoadPointService = procLoadPointService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（上料点表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcLoadPointDto>> QueryPagedProcLoadPointAsync([FromQuery] ProcLoadPointPagedQueryDto parm)
        {
            return await _procLoadPointService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（上料点表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcLoadPointDto> QueryProcLoadPointByIdAsync(long id)
        {
            return await _procLoadPointService.QueryProcLoadPointByIdAsync(id);
        }

        /// <summary>
        /// 添加（上料点表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddProcLoadPointAsync([FromBody] ProcLoadPointCreateDto parm)
        {
             await _procLoadPointService.CreateProcLoadPointAsync(parm);
        }

        /// <summary>
        /// 更新（上料点表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task UpdateProcLoadPointAsync([FromBody] ProcLoadPointModifyDto parm)
        {
             await _procLoadPointService.ModifyProcLoadPointAsync(parm);
        }

        /// <summary>
        /// 删除（上料点表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task DeleteProcLoadPointAsync(string ids)
        {
            //long[] idsArr = StringExtension.SpitLongArrary(ids);
            await _procLoadPointService.DeletesProcLoadPointAsync(ids);
        }

    }
}