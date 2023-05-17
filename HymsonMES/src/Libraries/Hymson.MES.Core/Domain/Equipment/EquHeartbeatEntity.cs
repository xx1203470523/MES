using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备心跳，数据实体对象   
    /// equ_heartbeat
    /// @author Czhipu
    /// @date 2023-05-16 01:45:02
    /// </summary>
    public class EquHeartbeatEntity : BaseEntity
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
        /// 最后在线时间
        /// </summary>
        public DateTime? LastOnLineTime { get; set; }

       /// <summary>
        /// 状态;0：离线 1、在线
        /// </summary>
        public bool Status { get; set; }

       
    }
}
