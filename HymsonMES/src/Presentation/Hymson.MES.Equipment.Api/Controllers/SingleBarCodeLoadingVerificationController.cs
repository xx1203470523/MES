using Hymson.MES.EquipmentServices.Request.SingleBarCodeLoadingVerification;
using Hymson.MES.EquipmentServices.Services.SingleBarCodeLoadingVerification;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 单体条码上料校验
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SingleBarCodeLoadingVerificationController : ControllerBase
    {
        private readonly ISingleBarCodeLoadingVerificationService _SingleBarCodeLoadingVerificationService;
        private readonly ILogger<SingleBarCodeLoadingVerificationController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SingleBarCodeLoadingVerificationService"></param>
        /// <param name="logger"></param>
        public SingleBarCodeLoadingVerificationController(ISingleBarCodeLoadingVerificationService SingleBarCodeLoadingVerificationService, ILogger<SingleBarCodeLoadingVerificationController> logger)
        {
            _SingleBarCodeLoadingVerificationService = SingleBarCodeLoadingVerificationService;
            _logger = logger;
        }

        /// <summary>
        /// 单体条码上料校验
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SingleBarCodeLoadingVerification")]
        public async Task SingleBarCodeLoadingVerificationAsync(SingleBarCodeLoadingVerificationRequest request)
        {
            await _SingleBarCodeLoadingVerificationService.SingleBarCodeLoadingVerificationAsync(request);
        }
    }
}
