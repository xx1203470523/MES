using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 条码打印 分页参数
    /// </summary>
    public class PlanSfcPrintPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

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
