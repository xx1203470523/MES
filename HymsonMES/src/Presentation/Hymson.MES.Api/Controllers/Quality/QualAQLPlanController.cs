using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（AQL检验计划）
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualAQLPlanController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualAQLPlanController> _logger;

        /// <summary>
        /// 服务接口（AQL检验计划）
        /// </summary>
        private readonly IQualAQLPlanService _qualAQLPlanService;


        /// <summary>
        /// 构造函数（AQL检验水平）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualAQLPlanService"></param>
        public QualAQLPlanController(ILogger<QualAQLPlanController> logger, IQualAQLPlanService qualAQLPlanService)
        {
            _logger = logger;
            _qualAQLPlanService = qualAQLPlanService;
        }


        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IEnumerable<QualAQLPlanExcelDto>> QueryListAsync()
        {
            return await _qualAQLPlanService.QueryListAsync();
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <returns></returns>
        [HttpGet("downloadImportTemplate")]
        [PermissionDescription("quality:qualAQLPlan:download")]
        [LogDescription("导入模板下载", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<IActionResult> DownloadTemplateExcel()
        {
            using MemoryStream stream = new();
            var worksheetName = await _qualAQLPlanService.DownloadImportTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{worksheetName}导入模板.xlsx");
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("import")]
        [LogDescription("导入", BusinessType.EXPORT)]
        [PermissionDescription("quality:qualAQLPlan:import")]
        public async Task ImportAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _qualAQLPlanService.ImportAsync(formFile);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        [PermissionDescription("quality:qualAQLPlan:export")]
        public async Task<ExportResponseDto> ExprotAsync([FromQuery] QualAQLPlanExprotRequestDto dto)
        {
            return await _qualAQLPlanService.ExprotAsync(dto);
        }

    }
}