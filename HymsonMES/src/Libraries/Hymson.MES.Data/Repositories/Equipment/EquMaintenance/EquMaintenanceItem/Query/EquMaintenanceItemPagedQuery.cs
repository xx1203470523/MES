using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.EquMaintenance.EquMaintenanceItem.Query
{
    /// <summary>
    /// 设备保养项目 分页参数
    /// </summary>
    public class EquMaintenanceItemPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
