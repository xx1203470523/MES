using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCreateBarcodeDto;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCreateBarcode;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 下达条码控制器
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuCreateBarcodeController : ControllerBase
    {
        private readonly IManuCreateBarcodeService _manuCreateBarcodeService;
        public ManuCreateBarcodeController(IManuCreateBarcodeService manuCreateBarcodeService)
        {
            _manuCreateBarcodeService = manuCreateBarcodeService;
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateBarcodeByWorkOrder")]
        public async Task GenerateBarcodeAsync(CreateBarcodeByWorkOrderDto parm)
        {
            await _manuCreateBarcodeService.CreateBarcodeByWorkOrderId(parm);
        }
    }
}