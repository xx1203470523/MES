using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（消息管理分析报告附件）   
    /// inte_message_manage_analysis_report_attachment
    /// @author Czhipu
    /// @date 2023-08-15 08:49:07
    /// </summary>
    public class InteMessageManageAnalysisReportAttachmentEntity : BaseEntity
    {
        /// <summary>
        /// 首检检验单Id
        /// </summary>
        public long MessageManageId { get; set; }

       /// <summary>
        /// 附件Id
        /// </summary>
        public long AttachmentId { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
