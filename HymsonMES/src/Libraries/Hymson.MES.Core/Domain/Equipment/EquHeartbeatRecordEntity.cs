using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备心跳记录，数据实体对象   
    /// equ_heartbeat_record
    /// @author Czhipu
    /// @date 2023-05-16 01:45:38
    /// </summary>
    public class EquHeartbeatRecordEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 状态;0：离线 1、在线
        /// </summary>
        public bool Status { get; set; }

       /// <summary>
        /// 传输时间
        /// </summary>
        public DateTime? LocalTime { get; set; }

       
    }
}
