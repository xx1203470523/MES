using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Dtos.Manufacture;

namespace Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode
{
    /// <summary>
    /// 根据老条码生成新条码
    /// </summary>
    public class CreateBarcodeByOldMesSFCBo : CoreBaseBo
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
        /// 旧条码
        /// </summary>
        public IEnumerable<BarcodeDto> OldSFCs { set; get; } = new List<BarcodeDto>();
    }
}
