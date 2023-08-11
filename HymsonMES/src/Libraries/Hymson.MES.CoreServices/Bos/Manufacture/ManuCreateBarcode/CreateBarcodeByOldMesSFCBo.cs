using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Dtos.Manufacture.ManuCommon.ManuCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// 就条码
        /// </summary>
        public IEnumerable<BarcodeDto> OldSFCs { set; get; } = new List<BarcodeDto>();
    }
}
