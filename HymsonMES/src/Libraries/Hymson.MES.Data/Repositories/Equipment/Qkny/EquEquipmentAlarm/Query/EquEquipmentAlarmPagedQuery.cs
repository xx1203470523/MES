using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.EquEquipmentAlarm.Query
{
    /// <summary>
    /// 设备报警记录 分页参数
    /// </summary>
    public class EquEquipmentAlarmPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
