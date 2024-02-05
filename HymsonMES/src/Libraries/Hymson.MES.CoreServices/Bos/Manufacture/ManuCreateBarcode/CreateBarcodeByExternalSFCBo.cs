using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Dtos.Manufacture.ManuCommon.ManuCommon;

namespace Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode
{
    /// <summary>
    /// 根据外部条码接收
    /// </summary>
    public class CreateBarcodeByExternalSFCBo : CoreBaseBo
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { set; get; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long? ResourceId { set; get; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { set; get; }

        /// <summary>
        /// 就条码
        /// </summary>
        public IEnumerable<BarcodeDto> ExternalSFCs { set; get; }=new List<BarcodeDto>();
    }
}
