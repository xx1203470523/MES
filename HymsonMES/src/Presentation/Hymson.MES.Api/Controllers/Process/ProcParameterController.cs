using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（标准参数表）
    /// </summary>
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
        /// <param name="logger"></param>
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
        [HttpGet]
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
        [LogDescription("标准参数", BusinessType.INSERT)]
        [PermissionDescription("proc:parameter:insert")]
        public async Task<long> AddProcParameterAsync([FromBody] ProcParameterCreateDto parm)
        {
            return await _procParameterService.CreateProcParameterAsync(parm);
        }

        /// <summary>
        /// 更新（标准参数表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("标准参数", BusinessType.UPDATE)]
        [PermissionDescription("proc:parameter:update")]
        public async Task UpdateProcParameterAsync([FromBody] ProcParameterModifyDto parm)
        {
            await _procParameterService.ModifyProcParameterAsync(parm);
        }

        /// <summary>
        /// 删除（标准参数表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("标准参数", BusinessType.DELETE)]
        [PermissionDescription("proc:parameter:delete")]
        public async Task<int> DeleteProcParameterAsync([FromBody] long[] ids)
        {
            return await _procParameterService.DeletesProcParameterAsync(ids);
        }

        /// <summary>
        /// 导入参数数据
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("import")]
        [LogDescription("导入参数数据", BusinessType.EXPORT)]
        [PermissionDescription("proc:parameter:import")]
        public async Task ImportParameterAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _procParameterService.ImportParameterAsync(formFile);
        }

        /// <summary>
        /// 导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("downloadImportTemplate")]
        [LogDescription("导入模板下载", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<IActionResult> DownloadTemplateExcel()
        {
            using MemoryStream stream = new MemoryStream();
            await _procParameterService.DownloadImportTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"参数导入模板.xlsx");
        }

        /// <summary>
        /// 导出标准参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        [LogDescription("导出标准参数信息", BusinessType.EXPORT)]
        [PermissionDescription("proc:parameter:export")]
        public async Task<ParameterExportResultDto> ExprotParameterListAsync([FromQuery] ProcParameterPagedQueryDto param)
        {
            return await _procParameterService.ExprotParameterListAsync(param);
        }
    }
}