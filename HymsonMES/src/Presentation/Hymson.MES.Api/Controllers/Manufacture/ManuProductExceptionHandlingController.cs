using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// �������������쳣����
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuProductExceptionHandlingController : ControllerBase
    {
        /// <summary>
        /// ��־����
        /// </summary>
        private readonly ILogger<ManuFeedingController> _logger;

        /// <summary>
        /// 
        /// </summary>
        private readonly IManuProductExceptionHandlingService _manuProductExceptionHandlingService;

        /// <summary>
        /// ���캯���������쳣����
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuProductExceptionHandlingService"></param>
        public ManuProductExceptionHandlingController(ILogger<ManuFeedingController> logger, IManuProductExceptionHandlingService manuProductExceptionHandlingService)
        {
            _logger = logger;
            _manuProductExceptionHandlingService = manuProductExceptionHandlingService;
        }


        /// <summary>
        /// ��ѯ���루���ѣ�
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [HttpGet("barCode/{barCode}")]
        public async Task<ManuBarCodeDto> GetBarCodeAsync(string barCode)
        {
            return await _manuProductExceptionHandlingService.GetBarCodeAsync(barCode);
        }

        /// <summary>
        /// �ύ�����ѣ�
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