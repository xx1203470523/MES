/*
 *describe: 工序表    控制器 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-02-13 09:06:05
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.IProcessService;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// 控制器（工序表）
    /// @author zhaoqing
    /// @date 2023-02-13 09:06:05
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcProcedureController : ControllerBase
    {
        /// <summary>
        /// 接口（工序表）
        /// </summary>
        private readonly IProcProcedureService _procProcedureService;
        private readonly ILogger<ProcProcedureController> _logger;

        /// <summary>
        /// 构造函数（工序表）
        /// </summary>
        /// <param name="procProcedureService"></param>
        public ProcProcedureController(IProcProcedureService procProcedureService, ILogger<ProcProcedureController> logger)
        {
            _procProcedureService = procProcedureService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（工序表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<PagedInfo<ProcProcedureViewDto>> QueryPagedProcProcedure([FromQuery] ProcProcedurePagedQueryDto parm)
        {
            return await _procProcedureService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（工序表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QueryProcProcedureDto> GetProcProcedureById(long id)
        {
            return await _procProcedureService.GetProcProcedureByIdAsync(id);
        }

        /// <summary>
        /// 获取工序配置打印信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("configPrint/list")]
        public async Task<PagedInfo<QueryProcProcedurePrintReleationDto>> GetProcedureBomConfigPrintListAsync([FromQuery] ProcProcedurePrintReleationPagedQueryDto parm)
        {
            return await _procProcedureService.GetProcedureConfigPrintListAsync(parm);
        }

        /// <summary>
        /// 获取工序配置Job信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("configJob/list")]
        public async Task<PagedInfo<QueryProcedureJobReleationDto>> GetProcedureBomConfigJobList([FromQuery] InteJobBusinessRelationPagedQueryDto parm)
        {
            return await _procProcedureService.GetProcedureConfigJobListAsync(parm);
        }

        /// <summary>
        /// 添加（工序表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddProcProcedureAsync([FromBody] AddProcProcedureDto parm)
        {
            await _procProcedureService.CreateProcProcedureAsync(parm);
        }

        /// <summary>
        /// 更新（工序表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateProcProcedureAsync([FromBody] UpdateProcProcedureDto parm)
        {
            await _procProcedureService.ModifyProcProcedureAsync(parm);
        }

        /// <summary>
        /// 删除（工序表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        public async Task DeleteProcProcedureAsync(string ids)
        {
            await _procProcedureService.DeleteProcProcedureAsync(ids);
        }

    }
}