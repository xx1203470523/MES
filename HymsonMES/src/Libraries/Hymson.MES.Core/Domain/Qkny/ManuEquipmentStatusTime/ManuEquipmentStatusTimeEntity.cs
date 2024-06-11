using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.ManuEquipmentStatusTime
{
    /// <summary>
    /// 数据实体（设备状态时间）   
    /// manu_equipment_status_time
    /// @author Yxx
    /// @date 2024-03-07 07:32:15
    /// </summary>
    public class ManuEquipmentStatusTimeEntity : BaseEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 当前状态
        /// </summary>
        public ManuEquipmentStatusEnum CurrentStatus { get; set; }

       /// <summary>
        /// 下一个状态
        /// </summary>
        public ManuEquipmentStatusEnum NextStatus { get; set; }

       /// <summary>
        /// 状态开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

       /// <summary>
        /// 状态结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

       /// <summary>
        /// 状态持续时间（单位秒）
        /// </summary>
        public int StatusDuration { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 设备停机原因
        /// </summary>
        public string EquipmentDownReason { get; set; }

       
    }
}
