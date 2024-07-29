using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 设备保养任务附件 分页参数
    /// </summary>
    public class EquMaintenanceTaskAttachmentPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
