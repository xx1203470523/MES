using Hymson.MES.EquipmentServices.Request.BindSFC;
using Hymson.MES.EquipmentServices.Services.BindSFC;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 条码绑定
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BindSFCController : ControllerBase
    {
        private readonly IBindSFCService _bindSFCService;
        private readonly ILogger<BindSFCController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bindSFCService"></param>
        /// <param name="logger"></param>
        public BindSFCController(IBindSFCService bindSFCService, ILogger<BindSFCController> logger)
        {
            _bindSFCService = bindSFCService;
            _logger = logger;
        }

        /// <summary>
        /// 绑定条码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BindSFC")]
        public async Task BindSFCAsync(BindSFCRequest request)
        {
            await _bindSFCService.BindSFCAsync(request);
        }
    }
}
