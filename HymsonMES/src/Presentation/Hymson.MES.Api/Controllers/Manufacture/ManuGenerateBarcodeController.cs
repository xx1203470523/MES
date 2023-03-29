using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 生成条码控制器
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuGenerateBarcodeController : ControllerBase
    {
        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;
        public ManuGenerateBarcodeController(IManuGenerateBarcodeService manuGenerateBarcodeService)
        {
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateBarcodeByCodeRuleId")]
        public async Task<IEnumerable<string>> GenerateBarcodeAsync(GenerateBarcodeDto parm)
        {
            return await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(parm);
        }


        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateBarcodeByCodeRuleData")]
        public async Task<IEnumerable<string>> GenerateBarcodeAsync(CodeRuleDto parm)
        {
            return await _manuGenerateBarcodeService.GenerateBarcodeListAsync(parm);
        }
    }
}