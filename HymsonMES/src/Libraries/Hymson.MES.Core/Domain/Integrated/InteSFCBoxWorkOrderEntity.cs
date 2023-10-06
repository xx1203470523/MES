using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Integrated
{
    public class InteSFCBoxWorkOrderEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long? Siteid { get; set; }

        /// <summary>
        /// 箱码(弃用)
        /// </summary>
        public string BoxCode { get; set; }

        /// <summary>
        /// 批次码
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 档位
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }
 
    }
}
