using Hymson.MES.Services.Services.Integrated;
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
        public async Task ImportWorkShopDataAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _importBasicDataService.ImportWorkShopDataAsync(formFile);
        }

        /// <summary>
        /// 车间数据导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("importMaterialGroup")]
        public async Task ImportMaterialGroupDataAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _importBasicDataService.ImportMaterialGroupDataAsync(formFile);
        }
    }
}