using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.Procedure;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minio.DataModel;

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
        [Route("print/list")]
        public async Task<PagedInfo<ProcProcedurePrintReleationDto>> GetProcedureBomConfigPrintListAsync([FromQuery] ProcProcedurePrintReleationPagedQueryDto parm)
        {
            return await _procProcedureService.GetProcedureConfigPrintListAsync(parm);
        }

        /// <summary>
        /// 获取工序配置Job信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("job/list")]
        public async Task<PagedInfo<ProcedureJobReleationDto>> GetProcedureBomConfigJobList([FromQuery] InteJobBusinessRelationPagedQueryDto parm)
        {
            return await _procProcedureService.GetProcedureConfigJobListAsync(parm);
        }

        /// <summary>
        /// 获取工序产出设置
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("product/list")]
        public async Task<IEnumerable<ProcProductSetDto>> GetProcedureProductSetListAsync([FromQuery] ProcProductSetQueryDto parm)
        {
            return await _procProcedureService.GetProcedureProductSetListAsync(parm);
        }

        /// <summary>
        /// 添加（工序表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("工序维护", BusinessType.INSERT)]
        [PermissionDescription("proc:procedure:insert")]
        public async Task AddProcProcedureAsync([FromBody] AddProcProcedureDto parm)
        {
            await _procProcedureService.AddProcProcedureAsync(parm);
        }

        /// <summary>
        /// 更新（工序表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("工序维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:procedure:update")]
        public async Task UpdateProcProcedureAsync([FromBody] UpdateProcProcedureDto parm)
        {
            await _procProcedureService.UpdateProcProcedureAsync(parm);
        }

        /// <summary>
        /// 删除（工序表）
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("工序维护", BusinessType.DELETE)]
        [PermissionDescription("proc:procedure:delete")]
        public async Task DeleteProcProcedureAsync(DeleteDto deleteDto)
        {
            await _procProcedureService.DeleteProcProcedureAsync(deleteDto.Ids);
        }

        #region PDA

        /// <summary>
        /// 获取所有工序
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("pda/list")]
        public async Task<IEnumerable<ProcProcedureViewPDADto>> GetProcProcedurePDA()
        {
            return await _procProcedureService.GetProcProcedurePDAAsync();
        }

        #endregion
    }
}