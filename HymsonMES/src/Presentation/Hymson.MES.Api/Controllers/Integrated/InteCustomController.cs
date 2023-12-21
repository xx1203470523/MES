using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（客户维护）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteCustomController : ControllerBase
    {
        /// <summary>
        /// 接口（客户维护）
        /// </summary>
        private readonly IInteCustomService _inteCustomService;

        private readonly ILogger<InteCustomController> _logger;

        /// <summary>
        /// 构造函数（客户维护）
        /// </summary>
        /// <param name="inteCustomService"></param>
        /// <param name="logger"></param>
        public InteCustomController(IInteCustomService inteCustomService, ILogger<InteCustomController> logger)
        {
            _inteCustomService = inteCustomService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（客户维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteCustomDto>> QueryPagedInteCustomAsync([FromQuery] InteCustomPagedQueryDto parm)
        {
            return await _inteCustomService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（客户维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteCustomDto> QueryInteCustomByIdAsync(long id)
        {
            return await _inteCustomService.QueryInteCustomByIdAsync(id);
        }

        /// <summary>
        /// 添加（客户维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("客户维护", BusinessType.INSERT)]
        [PermissionDescription("inte:inteCustom:insert")]
        public async Task<long> AddInteCustomAsync([FromBody] InteCustomCreateDto parm)
        {
            return await _inteCustomService.CreateInteCustomAsync(parm);
        }

        /// <summary>
        /// 更新（客户维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("客户维护", BusinessType.UPDATE)]
        [PermissionDescription("inte:inteCustom:update")]
        public async Task UpdateInteCustomAsync([FromBody] InteCustomModifyDto parm)
        {
             await _inteCustomService.ModifyInteCustomAsync(parm);
        }

        /// <summary>
        /// 删除（客户维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("客户维护", BusinessType.DELETE)]
        [PermissionDescription("inte:inteCustom:delete")]
        public async Task DeleteInteCustomAsync([FromBody] long[] ids)
        {
            await _inteCustomService.DeletesInteCustomAsync(ids);
        }

        #endregion

        /// <summary>
        /// 导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("downloadImportTemplate")]
        [LogDescription("导入模板下载", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<IActionResult> DownloadTemplateExcel()
        {
            using MemoryStream stream = new MemoryStream();
            await _inteCustomService.DownloadImportTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"客户维护导入模板.xlsx");
        }

        /// <summary>
        /// 导入客户数据
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("importCustom")]
        public async Task ImportCustomAsync([FromForm(Name = "file")] IFormFile formFile)
        {
 
            await _inteCustomService.ImportInteCustomAsync(formFile);
        }

        /// <summary>
        /// 导出客户维护信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        [PermissionDescription("proc:parameter:export")]
        public async Task<InteCustomExportResultDto> ExprotComUsagePageListAsync([FromQuery] InteCustomPagedQueryDto param)
        {
            return await _inteCustomService.ExprotInteCustomPageListAsync(param);
        }

    }
}