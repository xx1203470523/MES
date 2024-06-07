using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.ManuEquipmentStatusTime.Query
{
    /// <summary>
    /// 设备状态时间 分页参数
    /// </summary>
    public class ManuEquipmentStatusTimePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public long? EquipmentId { get; set; }

    }
}
