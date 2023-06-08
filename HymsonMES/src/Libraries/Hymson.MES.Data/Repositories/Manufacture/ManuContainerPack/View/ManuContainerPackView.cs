using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class ManuContainerPackView : BaseEntity
    {
        public long SiteId { get; set; }
        public long ContainerBarCodeId { get; set; }

        /// <summary>
        /// 容器条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 包装等级
        /// </summary>
        public int PackLevel { get; set; }

        /// <summary>
        /// 装载条码
        /// </summary>
        public string LadeBarCode { get; set; }
        public long ProductId { get; set; }
        /// <summary>
        /// 工单号
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 容器包装数量
        /// </summary>
        public int Count { get; set; }


    }
}
