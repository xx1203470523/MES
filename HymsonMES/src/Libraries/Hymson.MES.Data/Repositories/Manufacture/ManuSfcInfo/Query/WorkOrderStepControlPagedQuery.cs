/*
 *creator: Karl
 *
 *describe: 工单报告 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:00:29
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query
{
    /// <summary>
    /// 工单报告 分页参数
    /// </summary>
    public class WorkOrderStepControlPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public long? ProcessRouteId { get; set; }
    }
}
