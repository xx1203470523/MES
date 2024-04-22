using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.EquEquipmentAlarm
{
    /// <summary>
    /// 数据实体（设备报警记录）   
    /// equ_equipment_alarm
    /// @author Yxx
    /// @date 2024-03-08 09:11:19
    /// </summary>
    public class EquEquipmentAlarmEntity : BaseEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 0恢复;1发生;
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 报警详细信息
        /// </summary>
        public string AlarmMsg { get; set; }

        /// <summary>
        /// 报警代码
        /// </summary>
        public string AlarmCode { get; set; }

        /// <summary>
        /// L提示不停机;M提示停机;H故障停机;
        /// </summary>
        public string AlarmLevel { get; set; }

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
