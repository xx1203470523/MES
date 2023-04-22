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
        /// 装载条码
        /// </summary>
        public string LadeBarCode { get; set; }
        public long ProductId { get; set; }

        /// <summary>
        /// 装载人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 装载时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

    }
}
