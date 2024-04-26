using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCreateBarcodeDto;
using Hymson.Web.Framework.Attributes;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Plan;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 下达条码控制器
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuCreateBarcodeController : ControllerBase
    {
        private readonly IPlanSfcPrintService _planSfcPrintService;
        public ManuCreateBarcodeController(IPlanSfcPrintService planSfcPrintService)
        {
            _planSfcPrintService = planSfcPrintService;
        }

        /// <summary>
        /// 在条码下达时获取最新条码信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetNewBarCodeOnBarCodeCreatedAsync")]
        [LogDescription("在条码下达时获取最新条码信息", BusinessType.OTHER)]
        public async Task<IEnumerable<CreateBarcodeByWorkOrderOutputBo>> GetNewBarCodeOnBarCodeCreatedAsync(PlanSfcPrintQueryDto query)
        {
            return await _planSfcPrintService.GetNewBarCodeOnBarCodeCreatedAsync(query);
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateBarcodeByWorkOrder")]
        [LogDescription("生成条码", BusinessType.INSERT)]
        public async Task<List<CreateBarcodeByWorkOrderOutputBo>> GenerateBarcodeAsync(CreateBarcodeByWorkOrderDto parm)
        {
            return await _planSfcPrintService.CreateBarcodeByWorkOrderIdAsync(parm);
        }

        /// <summary>
        /// 生成条码并打印
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateBarcodeByWorkOrderAndPrint")]
        [LogDescription("生成条码并打印", BusinessType.OTHER)]
        public async Task GenerateBarcodeAndPrintAsync(CreateBarcodeByWorkOrderAndPrintDto parm)
        {
            await _planSfcPrintService.CreateBarcodeByWorkOrderIdAndPrintAsync(parm);
        }
    }
}