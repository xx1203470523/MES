using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（条码调整）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuBarcodeAdjustController : ControllerBase
    {
        private readonly ILogger<ManuDowngradingController> _logger;
        private readonly IManuBarcodeAdjustService _manuBarcodeAdjustService;

        /// <summary>
        /// 构造函数（条码调整）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuBarcodeAdjustService"></param>
        public ManuBarcodeAdjustController(ILogger<ManuDowngradingController> logger, IManuBarcodeAdjustService manuBarcodeAdjustService)
        {
            _logger = logger;

            _manuBarcodeAdjustService = manuBarcodeAdjustService;
        }

        /// <summary>
        /// 分页获取条码
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet("getBarcodePadedList")]
        public async Task<PagedInfo<ManuSfcAboutInfoViewDto>> GetBarcodePagedListAsync([FromQuery] ManuSfcAboutInfoPagedQueryDto queryDto)
        {
            return await _manuBarcodeAdjustService.GetBarcodePagedListAsync(queryDto);
        }

        /// <summary>
        /// 合并的 验证条码
        /// </summary>
        /// <param name="sfcs"></param>
        [HttpPost("mergeAdjustVerifySfcs")]
        [LogDescription("合并的 验证条码", BusinessType.INSERT)]
        public async Task<bool> MergeAdjustVerifySfcsAsync(string[] sfcs)
        {
            return await _manuBarcodeAdjustService.MergeAdjustVerifySfcAsync(sfcs);
        }

        /// <summary>
        /// 调整条码数量 验证条码
        /// </summary>
        /// <param name="sfcs"></param>
        [HttpPost("qtyAdjustVerifySfcs")]
        [LogDescription("调整条码数量 验证条码", BusinessType.UPDATE)]
        public async Task<bool> QtyAdjustVerifySfcsAsync(string[] sfcs)
        {
            return await _manuBarcodeAdjustService.QtyAdjustVerifySfcAsync(sfcs);
        }

        /// <summary>
        /// 合并的 获取条码
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("getSfcAboutInfoBySfcInMerge/{sfc}")]
        [LogDescription(" 合并的 获取条码", BusinessType.INSERT)]
        public async Task<ManuSfcAboutInfoViewDto?> GetSfcAboutInfoBySfcInMergeAsync(string sfc)
        {
            return await _manuBarcodeAdjustService.GetSfcAboutInfoBySfcInMergeAsync(sfc);
        }

        /// <summary>
        /// 条码数量调整的 获取条码
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("getSfcAboutInfoBySfcInQty/{sfc}")]
        public async Task<ManuSfcAboutInfoViewDto?> GetSfcAboutInfoBySfcInQtyAsync(string sfc)
        {
            return await _manuBarcodeAdjustService.GetSfcAboutInfoBySfcInQtyAsync(sfc);
        }

        /// <summary>
        /// Marking获取条码
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("getSfcAboutInfoBySfcInMarking/{sfc}")]
        public async Task<ManuSfcAboutInfoViewDto?> GetSfcAboutInfoBySfcInMarkingAsync(string sfc)
        {
            return await _manuBarcodeAdjustService.GetSfcAboutInfoByMarkingSfcAsync(sfc);
        }

        /// <summary>
        /// 条码数量调整
        /// </summary>
        /// <param name="adjustDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("barcodeQtyAdjust")]
        [LogDescription("条码数量调整", BusinessType.INSERT)]
        public async Task BarcodeQtyAdjustAsync(ManuBarcodeQtyAdjustDto adjustDto)
        {
            await _manuBarcodeAdjustService.BarcodeQtyAdjustAsync(adjustDto);
        }

        /// <summary>
        /// 条码拆分
        /// </summary>
        /// <param name="splitDto"></param>
        /// <returns></returns>
        [HttpPost("barcodeSplitAdjust")]
        [LogDescription("条码拆分", BusinessType.INSERT)]
        public async Task<string> BarCodeSplitAsync(ManuBarCodeSplitRequestDto splitDto)
        {
            return await _manuBarcodeAdjustService.BarCodeSplitAsync(splitDto);

        }

        /// <summary>
        /// 条码合并
        /// </summary>
        /// <param name="mergeDto"></param>
        /// <returns></returns>
        [HttpPost("barcodeMergeAdjust")]
        [LogDescription("条码合并", BusinessType.INSERT)]
        public async Task<string> BarCodeMergeAsync(ManuBarCodeMergeRequestDto mergeDto)
        {
            return await _manuBarcodeAdjustService.BarCodeMergeAsync(mergeDto);
        }
    }
}
