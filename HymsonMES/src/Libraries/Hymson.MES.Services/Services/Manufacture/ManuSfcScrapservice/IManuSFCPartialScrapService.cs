using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Services.Dtos.Manufacture.ManuSFCScrap;
using Microsoft.AspNetCore.Http;

namespace Hymson.MES.Services.Services.Manufacture.ManuSfcScrapservice
{
    /// <summary>
    /// 部分报废
    /// </summary>
    public interface IManuSFCPartialScrapService
    {
        /// <summary>
        /// 报废
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task PartialScrapAsync(ManuSFCPartialScrapDto param);

        /// <summary>
        /// 扫码校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task<PartialScrapBarCodeDto> BarcodeScanningAsync(PartialScrapScanningDto param);

        /// <summary>
        /// 模版下载
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<string> DownloadImportTemplateAsync(Stream stream);

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task ImportAsync(IFormFile formFile);
    }
}
