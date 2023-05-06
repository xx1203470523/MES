using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command
{
    /// <summary>
    /// 更新生产订单记录的实际结束时间
    /// </summary>
    public class UpdateWorkOrderRealEndCommand:UpdateCommand
    {
        /// <summary>
        /// 生产订单ID
        /// </summary>
        public long[] WorkOrderIds { get; set; }

    }
}
