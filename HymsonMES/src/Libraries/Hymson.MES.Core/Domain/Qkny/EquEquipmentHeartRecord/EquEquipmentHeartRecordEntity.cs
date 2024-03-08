using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.EquEquipmentHeartRecord
{
    /// <summary>
    /// 数据实体（设备心跳登录记录）   
    /// equ_equipment_heart_record
    /// @author Yxx
    /// @date 2024-03-07 03:39:54
    /// </summary>
    public class EquEquipmentHeartRecordEntity : BaseEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public string IsOnline { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }
}
