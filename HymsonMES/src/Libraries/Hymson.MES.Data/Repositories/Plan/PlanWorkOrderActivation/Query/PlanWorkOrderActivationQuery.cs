/*
 *creator: Karl
 *
 *describe: 工单激活 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-29 10:23:51
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单激活 查询参数
    /// </summary>
    public class PlanWorkOrderActivationQuery
    {
        /// <summary>
        /// 
        /// </summary>
        public long SiteId { get; set; }    

        /// <summary>
        /// 工单id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 线体id
        /// </summary>
        public long? LineId { get; set; }
    }
}