using Hymson.MES.Services.Services.Integrated;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（基础数据导入）
    /// @author zhaoqing
    /// @date 2024-02-29 04:30:52
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ImportBasicDataController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ImportBasicDataController> _logger;
        /// <summary>
        /// 服务接口（基础数据导入）
        /// </summary>
        private readonly IImportBasicDataService _importBasicDataService;

        /// <summary>
        /// 构造函数（基础数据导入）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="importBasicDataService"></param>
        public ImportBasicDataController(ILogger<ImportBasicDataController> logger, IImportBasicDataService importBasicDataService)
        {
            _logger = logger;
            _importBasicDataService = importBasicDataService;
        }

        /// <summary>
        /// 设备数据导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("importEqu")]
        [LogDescription("设备数据导入", BusinessType.EXPORT)]
        public async Task ImportEquDataAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _importBasicDataService.ImportEquDataAsync(formFile);
        }

        /// <summary>
        /// 资源数据导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("importResource")]
        [LogDescription("资源数据导入", BusinessType.EXPORT)]
        public async Task ImportResourceDataAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _importBasicDataService.ImportResourceDataAsync(formFile);
        }

        /// <summary>
        /// 资源类型数据导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("importResType")]
        [LogDescription("资源类型数据导入", BusinessType.EXPORT)]
        public async Task ImportResourceTypeDataAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _importBasicDataService.ImportResourceTypeDataAsync(formFile);
        }

        /// <summary>
        /// 工序数据导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("importProcedure")]
        [LogDescription("工序数据导入", BusinessType.EXPORT)]
        public async Task ImportProcedureDataAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _importBasicDataService.ImportProcedureDataAsync(formFile);
        }

        /// <summary>
        /// 产线数据导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("importWorkLine")]
        [LogDescription("产线数据导入", BusinessType.EXPORT)]
        public async Task ImportWorkLineDataAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _importBasicDataService.ImportWorkLineDataAsync(formFile);
        }

        /// <summary>
        /// 车间数据导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("importWorkShop")]
        [LogDescription("车间数据导入", BusinessType.EXPORT)]
        public async Task ImportWorkShopDataAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _importBasicDataService.ImportWorkShopDataAsync(formFile);
        }

        /// <summary>
        /// 物料组数据导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("importMaterialGroup")]
        [LogDescription("物料组数据导入", BusinessType.EXPORT)]
        public async Task ImportMaterialGroupDataAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _importBasicDataService.ImportMaterialGroupDataAsync(formFile);
        }

        /// <summary>
        /// 物料数据导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("importMaterial")]
        [LogDescription("物料数据导入", BusinessType.EXPORT)]
        public async Task ImportMaterialDataAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _importBasicDataService.ImportMaterialDataAsync(formFile);
        }

        /// <summary>
        /// 物料数据导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("importParameter")]
        [LogDescription("物料数据导入", BusinessType.EXPORT)]
        public async Task ImportParameterDataAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _importBasicDataService.ImportParameterDataAsync(formFile);
        }

        /// <summary>
        /// 物料导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("downloadMaterialTemplate")]
        [LogDescription("物料导入模板下载", BusinessType.EXPORT)]
        //[LogDescription("导入模板下载", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<IActionResult> DownloadMaterialTemplateAsync()
        {
            using MemoryStream stream = new MemoryStream();
            await _importBasicDataService.DownloadMaterialTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"物料导入模板.xlsx");
        }

        /// <summary>
        /// 参数导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("downloadParameterTTemplate")]
        [LogDescription("参数导入模板下载", BusinessType.EXPORT)]
        //[LogDescription("导入模板下载", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<IActionResult> DownloadParameterTemplateAsync()
        {
            using MemoryStream stream = new MemoryStream();
            await _importBasicDataService.DownloadParameterTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"参数导入模板.xlsx");
        }
    }
}