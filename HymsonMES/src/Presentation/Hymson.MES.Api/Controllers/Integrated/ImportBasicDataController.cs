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
        /// 基础数据导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("importEqu")]
        public async Task ImportEquDataAsync([FromForm(Name = "file")] IFormFile formFile)
        {
             await _importBasicDataService.ImportEquDataAsync(formFile);
        }
    }
}