using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture.ManuSFCScrap;
using Hymson.MES.Services.Services.Manufacture.ManuSfcScrapservice;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 物料报废控制器
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuMaterialScrapController : ControllerBase
    {
        /// <summary>
        /// 物料报废表
        /// </summary>
        private readonly IManuMaterialScrapService _manuMaterialScrapService;
        public ManuMaterialScrapController(IManuMaterialScrapService manuMaterialScrapService)
        {
            _manuMaterialScrapService = manuMaterialScrapService;
        }

        /// <summary>
        /// 扫码校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("BarcodeScanning")]

        public async Task<MaterialScrapBarCodeDto> BarcodeScanningAsync([FromQuery] MaterialScrapScanningDto param)
        {
            return await _manuMaterialScrapService.BarcodeScanningAsync(param);
        }

        /// <summary>
        /// 报废
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BarcodeScrap")]
        [LogDescription("物料报废表", BusinessType.OTHER)]
        public async Task PartialScrapAsync(ManuMaterialScrapDto param)
        {
            await _manuMaterialScrapService.ScrapAsync(param);
        }
    }
}
