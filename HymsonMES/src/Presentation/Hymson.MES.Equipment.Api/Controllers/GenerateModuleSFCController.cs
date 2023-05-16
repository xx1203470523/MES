using Hymson.MES.EquipmentServices.Request.GenerateModuleSFC;
using Hymson.MES.EquipmentServices.Services.GenerateModuleSFC;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 请求生成模组码-电芯堆叠
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GenerateModuleSFCController : ControllerBase
    {
        private readonly IGenerateModuleSFCService _GenerateModuleSFCService;
        private readonly ILogger<GenerateModuleSFCController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="GenerateModuleSFCService"></param>
        /// <param name="logger"></param>
        public GenerateModuleSFCController(IGenerateModuleSFCService GenerateModuleSFCService, ILogger<GenerateModuleSFCController> logger)
        {
            _GenerateModuleSFCService = GenerateModuleSFCService;
            _logger = logger;
        }

        /// <summary>
        /// 请求生成模组码-电芯堆叠
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateModuleSFC")]
        public async Task GenerateModuleSFCAsync(GenerateModuleSFCRequest request)
        {
            await _GenerateModuleSFCService.GenerateModuleSFCAsync(request);
        }
    }
}
