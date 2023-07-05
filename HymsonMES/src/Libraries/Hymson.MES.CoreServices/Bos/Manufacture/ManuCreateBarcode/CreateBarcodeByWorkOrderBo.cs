using Hymson.MES.CoreServices.Bos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode
{
    /// <summary>
    /// 根据工单下达条码
    /// </summary>
    public class CreateBarcodeByWorkOrderBo: CoreBaseBo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { set; get; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { set; get; }
    }
}
