/*
 *creator: Karl
 *
 *describe: 条码接收 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 条码接收 分页参数
    /// </summary>
    public class PlanSfcReceivePagedQuery : PagerInfo 
    {

        /// <summary>
        /// 工单号
        /// </summary>
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public long WorkOrderType { get; set; }

    }
}
