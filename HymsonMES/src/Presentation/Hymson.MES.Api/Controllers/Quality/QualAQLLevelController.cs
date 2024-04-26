using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（AQL检验水平）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualAQLLevelController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualAQLLevelController> _logger;

        /// <summary>
        /// 服务接口（AQL检验水平）
        /// </summary>
        private readonly IQualAQLLevelService _qualAQLLevelService;


        /// <summary>
        /// 构造函数（AQL检验水平）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualAQLLevelService"></param>
        public QualAQLLevelController(ILogger<QualAQLLevelController> logger, IQualAQLLevelService qualAQLLevelService)
        {
            _logger = logger;
            _qualAQLLevelService = qualAQLLevelService;
        }


        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IEnumerable<QualAQLLevelExcelDto>> QueryListAsync()
        {
            return await _qualAQLLevelService.QueryListAsync();
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <returns></returns>
        [HttpGet("downloadImportTemplate")]
        [PermissionDescription("quality:qualAQLLevel:download")]
        [LogDescription("下载导入模板", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<IActionResult> DownloadTemplateExcel()
        {
            using MemoryStream stream = new();
            var worksheetName = await _qualAQLLevelService.DownloadImportTemplateAsync(stream);
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
        [PermissionDescription("quality:qualAQLLevel:import")]
        public async Task ImportAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _qualAQLLevelService.ImportAsync(formFile);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        [PermissionDescription("quality:qualAQLLevel:export")]
        public async Task<ExportResponseDto> ExprotAsync([FromQuery] QualAQLLevelExprotRequestDto dto)
        {
            return await _qualAQLLevelService.ExprotAsync(dto);
        }

    }
}