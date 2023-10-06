using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated.InteSFCBox.Query
{
    public class PlanWorkOrderSFCBoxQuery
    {
        /// <summary>
        /// 关联表ID
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string? BatchNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedOn { get; set; }

    }
}
