using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Marking.Query
{
    /// <summary>
    /// Marking拦截汇总表 分页参数
    /// </summary>
    public class MarkingReportReportPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long? ProductId { get; set; }
        
        /// <summary>
        /// 产品序列码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 发现工序Id
        /// </summary>
        public long? FindProcedureId { get; set; }

        /// <summary>
        /// 拦截工序Id
        /// </summary>
        public long ?AppointInterceptProcedureId { get; set; }

        /// <summary>
        /// 实际拦截工序Id
        /// </summary>
        public long? InterceptProcedureId { get; set; }

        /// <summary>
        /// 拦截设备Id
        /// </summary>
        public long? InterceptEquipmentId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格状态
        /// </summary>
        public string UnqualifiedStatus { get; set; }

        /// <summary>
        /// Marking录入人员
        /// </summary>
        public string MarkingCreatedBy { get; set; }

        /// <summary>
        /// Marking录入时间
        /// </summary>
        public DateTime[]? MarkingCreatedOn { get; set; }

        /// <summary>
        /// Marking关闭人员
        /// </summary>
        public string MarkingClosedBy { get; set; }

        /// <summary>
        /// Marking录入时间
        /// </summary>
        public DateTime[]? MarkingCloseOn { get; set; }

        /// <summary>
        /// Marking拦截时间
        /// </summary>
        public DateTime[]? InterceptOn { get; set; }

    }
}
