using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Report.Query
{
    /// <summary>
    /// 降级品明细报表 分页参数
    /// </summary>
    public class ManuDowngradingDetailReportPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? OrderId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 降级等级
        /// </summary>
        public string? DowngradingCode { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

    }
}
