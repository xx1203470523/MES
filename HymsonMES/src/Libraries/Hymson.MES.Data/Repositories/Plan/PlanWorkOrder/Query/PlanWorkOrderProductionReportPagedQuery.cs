using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query
{
    public class PlanWorkOrderProductionReportPagedQuery : PagerInfo
    {
        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum? OrderType { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 产线编码
        /// </summary>
        public string? WorkCentCode { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? RealStart { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? RealEnd { get; set; }
    }
}
