/*
 *creator: Karl
 *
 *describe: 条码接收 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 条码接收 查询参数
    /// </summary>
    public class PlanSfcReceiveQuery
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// SFC
        /// </summary>
        public string? SFC { get; set; }
    }
}
