/*
 *creator: Karl
 *
 *describe: BOM表    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-14 10:04:25
 */
using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（BOM表）
    /// @author Karl
    /// @date 2023-02-14 10:04:25
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcBomController : ControllerBase
    {
        /// <summary>
        /// 接口（BOM表）
        /// </summary>
        private readonly IProcBomService _procBomService;
        private readonly ILogger<ProcBomController> _logger;

        /// <summary>
        /// 构造函数（BOM表）
        /// </summary>
        /// <param name="procBomService"></param>
        public ProcBomController(IProcBomService procBomService, ILogger<ProcBomController> logger)
        {
            _procBomService = procBomService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（BOM表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcBomDto>> QueryPagedProcBomAsync([FromQuery] ProcBomPagedQueryDto parm)
        {
            return await _procBomService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（BOM表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcBomDto> QueryProcBomByIdAsync(long id)
        {
            return await _procBomService.QueryProcBomByIdAsync(id);
        }

        /// <summary>
        /// 查询Bom维护表详情
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("material/list")]
        public async Task<List<ProcBomDetailView>> GetProcBomMaterialAsync(ProcBomMaterialQueryDto query)
        {
            var bomId = query?.BomId ?? 0;
            return await _procBomService.GetProcBomMaterialAsync(bomId);
        }

        /// <summary>
        /// 添加（BOM表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddProcBomAsync([FromBody] ProcBomCreateDto parm)
        {
             await _procBomService.CreateProcBomAsync(parm);
        }

        /// <summary>
        /// 更新（BOM表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task UpdateProcBomAsync([FromBody] ProcBomModifyDto parm)
        {
             await _procBomService.ModifyProcBomAsync(parm);
        }

        /// <summary>
        /// 删除（BOM表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task DeleteProcBomAsync(string ids)
        {
            //long[] idsArr = StringExtension.SpitLongArrary(ids);
            await _procBomService.DeletesProcBomAsync(ids);
        }

    }
}