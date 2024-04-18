using Hymson.Localization.Services;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode
{
    /// <summary>
    /// 
    /// </summary>
    public interface IManuCreateBarcodeService
    {
        /// <summary>
        /// 工单下达条码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<CreateBarcodeByWorkOrderOutputBo>> CreateBarcodeByWorkOrderIdAsync(CreateBarcodeByWorkOrderBo param, ILocalizationService localizationService);

        /// <summary>
        /// 根据外部条码接收
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task CreateBarcodeByExternalSFCAsync(CreateBarcodeByExternalSFCBo param, ILocalizationService localizationService);

        /// <summary>
        /// 内部条码复用
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task CreateBarcodeByOldMESSFCAsync(CreateBarcodeByOldMesSFCBo param, ILocalizationService localizationService);

        /// <summary>
        /// 生产中生成条码
        /// </summary>
        /// <param name="param"></param>
        /// <param name="localizationService"></param>
        /// <returns></returns>
        Task<IEnumerable<CreateBarcodeByWorkOrderOutputBo>> CreateBarcodeInProductionAsync(CreateBarcodeInProductionBo param);

        /// <summary>
        /// 生成条码（半成品）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<ManuSfcEntity>> CreateBarcodeBySemiProductIdAsync(CreateBarcodeByResourceCode param);

        /// <summary>
        /// 生成条码（电芯）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<ManuSfcEntity>> CreateCellBarCodeAsync(CreateBarcodeByResourceCode param);
    }
}
