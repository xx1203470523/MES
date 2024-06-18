using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备报警信息，数据实体对象   
    /// equ_alarm
    /// @author Czhipu
    /// @date 2023-05-16 04:51:15
    /// </summary>
    public class EquAlarmEntity : BaseEntity
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
        /// 故障代码
        /// </summary>
        public string FaultCode { get; set; }

        /// <summary>
        /// 故障信息
        /// </summary>
        public string AlarmMsg { get; set; }

        /// <summary>
        /// 状态;1、开启 2、恢复
        /// </summary>
        public EquipmentAlarmStatusEnum? Status { get; set; }

        /// <summary>
        /// 传输时间
        /// </summary>
        public DateTime? LocalTime { get; set; }


    }
}
