/*
 *creator: Karl
 *
 *describe: 物料维护    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-07 11:16:51
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（物料维护）
    /// @author Karl
    /// @date 2023-02-07 11:16:51
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcMaterialController : ControllerBase
    {
        /// <summary>
        /// 接口（物料维护）
        /// </summary>
        private readonly IProcMaterialService _procMaterialService;
        private readonly ILogger<ProcMaterialController> _logger;

        /// <summary>
        /// 构造函数（物料维护）
        /// </summary>
        /// <param name="procMaterialService"></param>
        public ProcMaterialController(IProcMaterialService procMaterialService, ILogger<ProcMaterialController> logger)
        {
            _procMaterialService = procMaterialService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（物料维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcMaterialDto>> QueryPagedProcMaterialAsync([FromQuery] ProcMaterialPagedQueryDto parm)
        {
            return await _procMaterialService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（物料维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcMaterialDto> QueryProcMaterialByIdAsync(long id)
        {
            return await _procMaterialService.QueryProcMaterialByIdAsync(id);
        }

        /// <summary>
        /// 添加（物料维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddProcMaterialAsync([FromBody] ProcMaterialCreateDto parm)
        {
             await _procMaterialService.CreateProcMaterialAsync(parm);
        }

        /// <summary>
        /// 更新（物料维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task UpdateProcMaterialAsync([FromBody] ProcMaterialModifyDto parm)
        {
             await _procMaterialService.ModifyProcMaterialAsync(parm);
        }

        /// <summary>
        /// 删除（物料维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task DeleteProcMaterialAsync(string ids)
        {
            long[] idsArr = StringExtension.SpitLongArrary(ids);
            await _procMaterialService.DeletesProcMaterialAsync(idsArr);
        }

    }
}