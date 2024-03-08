using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.EquEquipmentHeartRecord.Query
{
    /// <summary>
    /// 设备心跳登录记录 分页参数
    /// </summary>
    public class EquEquipmentHeartRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
