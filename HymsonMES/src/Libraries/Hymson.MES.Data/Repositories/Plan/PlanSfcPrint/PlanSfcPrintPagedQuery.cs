/*
 *creator: Karl
 *
 *describe: 条码打印 分页查询类 | 代码由框架生成
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
    /// 条码打印 分页参数
    /// </summary>
    public class PlanSfcPrintPagedQuery : PagerInfo
    {

        /// <summary>
        /// 工单号
        /// </summary>
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// SFC 
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 使用状态
        /// </summary>
        public long IsUsed { get; set; }

        /// <summary>
        /// 打印状态
        /// </summary>
        public long PrintStatus { get; set; }

    }
}
