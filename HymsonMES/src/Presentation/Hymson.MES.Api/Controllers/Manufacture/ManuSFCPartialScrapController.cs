using Hymson.MES.Services.Dtos.Manufacture.ManuSFCScrap;
using Hymson.MES.Services.Services.Manufacture.ManuSfcScrapservice;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 部分报废
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuSFCPartialScrapController : ControllerBase
    {
        /// <summary>
        /// 物料表仓储
        /// </summary>
        private readonly IManuSFCPartialScrapService _manuSFCPartialScrapService;
        public ManuSFCPartialScrapController(IManuSFCPartialScrapService manuSFCPartialScrapService)
        {
            _manuSFCPartialScrapService = manuSFCPartialScrapService;
        }

        /// <summary>
        /// 扫码校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("BarcodeScanning")]
        public async Task<PartialScrapBarCodeDto> BarcodeScanningAsync([FromQuery] PartialScrapScanningDto param)
        {
            return await _manuSFCPartialScrapService.BarcodeScanningAsync(param);
        }

        /// <summary>
        /// 报废
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PartialScrap")]
        [PermissionDescription("manufacture:manuSFCPartialScrap:PartialScrap")]
        [LogDescription("部分报废", BusinessType.INSERT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task PartialScrapAsync(ManuSFCPartialScrapDto param)
        {
            await _manuSFCPartialScrapService.PartialScrapAsync(param);
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <returns></returns>
        [HttpGet("download")]
        //[PermissionDescription("manufacture:manuSFCPartialScrap:download")]
        [LogDescription("导入模板下载", BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        public async Task<IActionResult> DownloadTemplateExcel()
        {
            using MemoryStream stream = new();
            var worksheetName = await _manuSFCPartialScrapService.DownloadImportTemplateAsync(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{worksheetName}导入模板.xlsx");
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("import")]
        //[PermissionDescription("manufacture:manuSFCPartialScrap:import")]
        public async Task ImportAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _manuSFCPartialScrapService.ImportAsync(formFile);
        }
    }
}
