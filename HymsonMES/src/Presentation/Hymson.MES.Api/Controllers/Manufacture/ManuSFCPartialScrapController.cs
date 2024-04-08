using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture.ManuSFCScrap;
using Hymson.MES.Services.Services.Manufacture.ManuSfcScrapservice;
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
        public async Task PartialScrapAsync(ManuSFCPartialScrapDto param)
        {
            await _manuSFCPartialScrapService.PartialScrapAsync(param);
        }
    }
}
