using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 生成条码控制器
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuGenerateBarcodeController : ControllerBase
    {
        private readonly IManuGenerateBarcodeExampleService _manuGenerateBarcodeService;

        public ManuGenerateBarcodeController(IManuGenerateBarcodeExampleService manuGenerateBarcodeService)
        {
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("generateBarcodeByCodeRuleId")]
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
        [Route("generateBarcodeByCodeRuleData")]
        public async Task<IEnumerable<string>> GenerateBarcodeAsync(CodeRuleDto parm)
        {
            return await _manuGenerateBarcodeService.GenerateBarcodeListAsync(parm);
        }
        /// <summary>
        /// 生成通配符列表
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("BarcodeWildcardItems")]
        public IEnumerable<BarcodeWildcardItemDto> GetGenerateBarcodeWildcardItemDtos()
        {
            return _manuGenerateBarcodeService.GetGenerateBarcodeWildcardItemDtos();
        }
    }
}