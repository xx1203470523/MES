using Hymson.MES.Core.Domain.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan.PlanWorkPlan.View
{
    /// <summary>
    /// ERP工单和ERP产品ID
    /// </summary>
    public class PlanWorkPorductView : PlanWorkPlanEntity
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public long ErpProductId { get; set; }
    }
}
