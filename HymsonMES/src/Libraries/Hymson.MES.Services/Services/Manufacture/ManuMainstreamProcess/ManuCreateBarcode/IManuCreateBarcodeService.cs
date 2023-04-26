using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCreateBarcodeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Task CreateBarcodeByWorkOrderId(CreateBarcodeByWorkOrderDto param);

        /// <summary>
        /// 根据外部条码下达条码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task CreateBarcodeByExternalSFC(CreateBarcodeByExternalSFCDto param);

        /// <summary>
        /// 内部条码复用
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task CreateBarcodeByOldMESSFC(CreateBarcodeByOldMesSFCDto param);
    }
}
