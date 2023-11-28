using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.QualificationRateReport.Query
{
    /// <summary>
    /// 合格率报表 分页参数
    /// </summary>
    public class QualificationRateReportPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public long[]? WorkOrderIds { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long[]? ProcedureIds { get; set; }

        /// <summary>
        /// 查询日期类型（日月年）
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime[]? Date { get; set; }
    }

    public class QualificationRateReportEnity : BaseEntity
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 产品信息
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartOn { get; set; }

        /// <summary>
        /// 开始小时
        /// </summary>
        public int? StartHour { get; set; }

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal QualifiedQuantity { get; set; }

        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal UnQualifiedQuantity { get; set; }
    }
}
