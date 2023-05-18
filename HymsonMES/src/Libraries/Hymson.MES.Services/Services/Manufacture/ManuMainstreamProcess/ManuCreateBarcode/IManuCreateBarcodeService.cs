using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCreateBarcodeDto;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCreateBarcode
{
    /// <summary>
    /// 下达条码
    /// </summary>
    public interface IManuCreateBarcodeService
    {
        /// <summary>
        /// 工单下达条码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<ManuSfcEntity>> CreateBarcodeByWorkOrderIdAsync(CreateBarcodeByWorkOrderDto param);
        /// <summary>
        /// 工单下达条码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task CreateBarcodeByWorkOrderIdAndPrintAsync(CreateBarcodeByWorkOrderAndPrintDto param);

        /// <summary>
        /// 根据外部条码下达条码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task CreateBarcodeByExternalSFCAsync(CreateBarcodeByExternalSFCDto param);

        /// <summary>
        /// 内部条码复用
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task CreateBarcodeByOldMESSFCAsync(CreateBarcodeByOldMesSFCDto param);
    }
}
