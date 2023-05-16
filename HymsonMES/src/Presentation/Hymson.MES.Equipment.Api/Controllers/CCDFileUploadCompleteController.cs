using Hymson.MES.EquipmentServices.Request.CCDFileUploadComplete;
using Hymson.MES.EquipmentServices.Services.CCDFileUploadComplete;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// CCD文件上传完成
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CCDFileUploadCompleteController : ControllerBase
    {
        private readonly ICCDFileUploadCompleteService _CCDFileUploadCompleteService;
        private readonly ILogger<CCDFileUploadCompleteController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="CCDFileUploadCompleteService"></param>
        /// <param name="logger"></param>
        public CCDFileUploadCompleteController(ICCDFileUploadCompleteService CCDFileUploadCompleteService, ILogger<CCDFileUploadCompleteController> logger)
        {
            _CCDFileUploadCompleteService = CCDFileUploadCompleteService;
            _logger = logger;
        }

        /// <summary>
        /// CCD文件上传完成
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CCDFileUploadComplete")]
        public async Task CCDFileUploadCompleteAsync(CCDFileUploadCompleteRequest request)
        {
            await _CCDFileUploadCompleteService.CCDFileUploadCompleteAsync(request);
        }
    }
}
