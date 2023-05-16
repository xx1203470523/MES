using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备状态记录表，数据实体对象   
    /// equ_status_record
    /// @author Czhipu
    /// @date 2023-05-16 04:51:46
    /// </summary>
    public class EquipmentStatusRecordEntity : BaseEntity
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
        /// 状态;0.自动运行、1.手动运行、2.停机、3.故障、4.离线
        /// </summary>
        public bool EquipmentStatus { get; set; }

        /// <summary>
        /// 停机原因
        /// </summary>
        public string LossRemark { get; set; }

        /// <summary>
        /// 设备停机开始时间
        /// </summary>
        public DateTime? HaltBeginTime { get; set; }

        /// <summary>
        /// 设备停机开始时间
        /// </summary>
        public DateTime? HaltEndTime { get; set; }

        /// <summary>
        /// 传输时间
        /// </summary>
        public DateTime? LocalTime { get; set; }


    }
}
