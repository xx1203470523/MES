using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquToolingManage;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// 控制器（工具管理表）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquToolingequManageController : ControllerBase
    {
        /// <summary>
        /// 接口（工具管理表）
        /// </summary>
        private readonly IEquToolingManageService _equToolingManageService;
        private readonly ILogger<EquToolingequManageController> _logger;

        /// <summary>
        /// 构造函数（工具管理表）
        /// </summary>
        /// <param name="equToolingManageService"></param>
        /// <param name="logger"></param>
        public EquToolingequManageController(IEquToolingManageService equToolingManageService, ILogger<EquToolingequManageController> logger)
        {
            _equToolingManageService = equToolingManageService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（工具管理表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<PagedInfo<EquToolingManageViewDto>> QueryEquToolingManage([FromQuery] EquToolingManagePagedQueryDto parm)
        {
            return await _equToolingManageService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（工具管理表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquToolingManageViewDto> QueryEquToolingManageByIdAsync(long id)
        {
            return await _equToolingManageService.QueryEquToolingManageByIdAsync(id);
        }

        /// <summary>
        /// 新增（工具管理表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("工具管理", BusinessType.INSERT)]
        [PermissionDescription("equ:toolingManage:create")]
        public async Task<long> AddEquToolingManageAsync([FromBody] AddEquToolingManageDto parm)
        {
            return await _equToolingManageService.AddEquToolingManageAsync(parm);
        }

        /// <summary>
        /// 删除（工具管理表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("工具管理", BusinessType.DELETE)]
        [PermissionDescription("equ:toolingManage:delete")]
        public async Task DeleteEquToolingManageAsync([FromBody] long[] ids)
        {
            await _equToolingManageService.DeleteEquToolingManageAsync(ids);
        }

        /// <summary>
        /// 校准
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/calibration")]
        [LogDescription("工具校准", BusinessType.UPDATE)]
        [PermissionDescription("equ:toolingManage:calibration")]
        public async Task CalibrationAsync(long id)
        { 
            await _equToolingManageService.CalibrationAsync(id);
        }

        /// <summary>
        /// 更新（工具管理表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("工具管理", BusinessType.UPDATE)]
        [PermissionDescription("equ:toolingManage:update")]
        public async Task ModifyEquToolingManageAsync([FromBody] EquToolingManageModifyDto parm)
        {
            await _equToolingManageService.ModifyEquToolingManageAsync(parm);
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <returns></returns>
        [HttpGet("downloadImportTemplate")]
        [PermissionDescription("equ:toolingManage:download")]
        [LogDescription("导入模板下载", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<IActionResult> DownloadTemplateExcel()
        {
            using MemoryStream stream = new();
            var worksheetName = await _equToolingManageService.DownloadImportTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{worksheetName}导入模板.xlsx");
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("import")]
        [PermissionDescription("equ:toolingManage:import")]
        public async Task ImportAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _equToolingManageService.ImportAsync(formFile);
        }
    }
}