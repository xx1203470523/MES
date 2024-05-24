using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.Procedure;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// 控制器（打印设置表）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcPrintSetupController : ControllerBase
    {
        /// <summary>
        /// 接口（打印设置表）
        /// </summary>
        private readonly IProcPrintSetupService _procPrintSetupService;
        private readonly ILogger<ProcPrintSetupController> _logger;

        /// <summary>
        /// 构造函数（打印设置表）
        /// </summary>
        /// <param name="procPrintSetupService"></param>
        /// <param name="logger"></param>
        public ProcPrintSetupController(IProcPrintSetupService procPrintSetupService, ILogger<ProcPrintSetupController> logger)
        {
            _procPrintSetupService = procPrintSetupService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（打印设置表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<PagedInfo<ProcPrintSetupViewDto>> QueryPagedProcProcedure([FromQuery] ProcPrintSetupPagedQueryDto parm)
        {
            return await _procPrintSetupService.GetPageListAsync(parm);
        }


        /// <summary>
        /// 查询详情（打印设置表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcPrintSetupDto> QueryProcPrintSetupByIdAsync(long id)
        {
            return await _procPrintSetupService.QueryProcPrintSetupByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（打印设置表）
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        [HttpGet("{materialId}/materialIdList")]
        public async Task<ProcPrintSetupDto> QueryProcPrintSetupByMaterialIdAsync(long materialId)
        {
            return await _procPrintSetupService.QueryProcPrintSetupByMaterialIdAsync(materialId);
        }

        /// <summary>
        /// 新增（打印设置表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("打印设置维护", BusinessType.INSERT)]
        [PermissionDescription("proc:PrintSetup:insert")]
        public async Task<long> AddProcPrintSetupAsync([FromBody] AddPrintSetupDto parm)
        {
            return await _procPrintSetupService.AddProcPrintSetupAsync(parm);
        }


        /// <summary>
        /// 删除（打印设置表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("打印设置维护", BusinessType.DELETE)]
        [PermissionDescription("proc:PrintSetup:delete")]
        public async Task DeleteProcPrintSetupAsync([FromBody] long[] ids)
        {
            await _procPrintSetupService.DeleteProcPrintSetupAsync(ids);
        }

        /// <summary>
        /// 更新（打印设置表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("打印设置维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:PrintSetup:update")]
        public async Task UpdateProcPrintSetupAsync([FromBody] ProcPrintSetupModifyDto parm)
        {
            await _procPrintSetupService.ModifyProcPrintSetupAsync(parm);
        }
    }
}