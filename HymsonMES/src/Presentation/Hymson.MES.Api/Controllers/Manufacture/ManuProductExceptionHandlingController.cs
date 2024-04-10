using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
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


        #region �ò�����
        /// <summary>
        /// ���������ѯ��Ϣ���ò����գ�
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [HttpGet("compromise/{barCode}")]
        public async Task<IEnumerable<ManuCompromiseBarCodeDto>> GetCompromiseByBarCodeAsync(string barCode)
        {
            return await _manuProductExceptionHandlingService.GetCompromiseByBarCodeAsync(barCode);
        }

        /// <summary>
        /// �ύ���ò����գ�
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("compromise/submit")]
        public async Task<int> SubmitCompromiseAsync(ManuCompromiseDto requestDto)
        {
            return await _manuProductExceptionHandlingService.SubmitCompromiseAsync(requestDto);
        }

        /// <summary>
        /// ���ص���ģ�壨�ò����գ�
        /// </summary>
        /// <returns></returns>
        [HttpGet("compromise/download")]
        //[PermissionDescription("manufacture:manuProductExceptionHandling:download")]
        [LogDescription("����ģ������", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<IActionResult> DownloadCompromiseImportTemplateAsync()
        {
            using MemoryStream stream = new();
            var worksheetName = await _manuProductExceptionHandlingService.DownloadCompromiseImportTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{worksheetName}����ģ��.xlsx");
        }

        /// <summary>
        /// ���루�ò����գ�
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("compromise/import")]
        //[PermissionDescription("manufacture:manuProductExceptionHandling:import")]
        public async Task ImportCompromiseAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _manuProductExceptionHandlingService.ImportCompromiseAsync(formFile);
        }
        #endregion


        #region �豸����
        /// <summary>
        /// ���������ѯ��Ϣ���豸���У�
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [HttpGet("misjudgment/{barCode}")]
        public async Task<IEnumerable<ManuMisjudgmentBarCodeDto>> GetMisjudgmentByBarCodeAsync(string barCode)
        {
            return await _manuProductExceptionHandlingService.GetMisjudgmentByBarCodeAsync(barCode);
        }

        /// <summary>
        /// �ύ���豸���У�
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("misjudgment/submit")]
        public async Task<int> SubmitMisjudgmentAsync(ManuMisjudgmentDto requestDto)
        {
            return await _manuProductExceptionHandlingService.SubmitMisjudgmentAsync(requestDto);
        }
        #endregion


        #region ����
        /// <summary>
        /// ���������ѯ��Ϣ��������
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [HttpGet("rework/{barCode}")]
        public async Task<IEnumerable<ManuReworkBarCodeDto>> GetReworkByBarCodeAsync(string barCode)
        {
            return await _manuProductExceptionHandlingService.GetReworkByBarCodeAsync(barCode);
        }

        /// <summary>
        /// ���������������ѯ��Ϣ��������
        /// </summary>
        /// <param name="palletCode"></param>
        /// <returns></returns>
        [HttpGet("rework/palletCode/{palletCode}")]
        public async Task<IEnumerable<ManuReworkBarCodeDto>> GetReworkByPalletCodeAsync(string palletCode)
        {
            return await _manuProductExceptionHandlingService.GetReworkByPalletCodeAsync(palletCode);
        }

        /// <summary>
        /// �ύ��������
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("rework/submit")]
        public async Task<int> SubmitReworkAsync(ManuReworkDto requestDto)
        {
            return await _manuProductExceptionHandlingService.SubmitReworkAsync(requestDto);
        }

        /// <summary>
        /// ���ص���ģ�壨������
        /// </summary>
        /// <returns></returns>
        [HttpGet("rework/download")]
        //[PermissionDescription("manufacture:manuProductExceptionHandling:download")]
        [LogDescription("����ģ������", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<IActionResult> DownloadImportReworkTemplateAsync()
        {
            using MemoryStream stream = new();
            var worksheetName = await _manuProductExceptionHandlingService.DownloadImportReworkTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{worksheetName}����ģ��.xlsx");
        }

        /// <summary>
        /// ���루������
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("rework/import")]
        //[PermissionDescription("manufacture:manuProductExceptionHandling:import")]
        public async Task ImportReworkAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _manuProductExceptionHandlingService.ImportReworkAsync(formFile);
        }
        #endregion


        #region ����
        /// <summary>
        /// ���������ѯ��Ϣ�����ѣ�
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [HttpGet("detachment/{barCode}")]
        public async Task<ManuDetachmentBarCodeDto> GetDetachmentByBarCodeAsync(string barCode)
        {
            return await _manuProductExceptionHandlingService.GetDetachmentByBarCodeAsync(barCode);
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
        #endregion


    }
}