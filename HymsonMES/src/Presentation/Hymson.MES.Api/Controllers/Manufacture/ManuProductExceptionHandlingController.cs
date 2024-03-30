using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（生产异常处理）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuProductExceptionHandlingController : ControllerBase
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<ManuFeedingController> _logger;

        /// <summary>
        /// 
        /// </summary>
        private readonly IManuProductExceptionHandlingService _manuProductExceptionHandlingService;

        /// <summary>
        /// 构造函数（生产异常处理）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuProductExceptionHandlingService"></param>
        public ManuProductExceptionHandlingController(ILogger<ManuFeedingController> logger, IManuProductExceptionHandlingService manuProductExceptionHandlingService)
        {
            _logger = logger;
            _manuProductExceptionHandlingService = manuProductExceptionHandlingService;
        }


        /// <summary>
        /// 查询条码（离脱）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [HttpGet("barCode/{barCode}")]
        public async Task<ManuBarCodeDto> GetBarCodeAsync(string barCode)
        {
            return await _manuProductExceptionHandlingService.GetBarCodeAsync(barCode);
        }

        /// <summary>
        /// 提交（离脱）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("detachment/submit")]
        public async Task<int> SubmitDetachmentAsync(ManuDetachmentDto requestDto)
        {
            return await _manuProductExceptionHandlingService.SubmitDetachmentAsync(requestDto);
        }

    }
}