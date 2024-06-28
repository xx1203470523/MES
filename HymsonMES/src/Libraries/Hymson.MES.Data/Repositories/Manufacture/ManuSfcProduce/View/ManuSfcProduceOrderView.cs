using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class ManuSfcProduceOrderView
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long? WorkCenterId { get; set; }
    }
}
