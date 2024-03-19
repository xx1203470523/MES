using Hymson.MES.CoreServices.Bos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode
{
    /// <summary>
    /// 生产中生成条码
    /// </summary>
    public class CreateBarcodeInProductionBo : CoreBaseBo
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { set; get; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { set; get; }

        /// <summary>
        /// 条码次数
        /// </summary>
        public int Count { set; get; } = 1;

        /// <summary>
        ///是否置于活动中
        /// </summary>
        public bool IsInActive { set; get; } = false;
    }
}
