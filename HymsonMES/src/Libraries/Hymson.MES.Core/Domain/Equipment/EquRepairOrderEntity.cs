/*
 *creator: Karl
 *
 *describe: 设备维修记录    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.EquRepairOrder
{
    /// <summary>
    /// 设备维修记录，数据实体对象   
    /// equ_repair_order
    /// @author pengxin
    /// @date 2024-06-12 10:56:10
    /// </summary>
    public class EquRepairOrderEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 维修
        /// </summary>
        public string RepairOrder { get; set; }

       /// <summary>
        /// 设备id equ_equipment的 id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 设备记录id equ_equipment_record
        /// </summary>
        public long EquipmentRecordId { get; set; }

       /// <summary>
        /// 故障时间
        /// </summary>
        public DateTime FaultTime { get; set; }

       /// <summary>
        /// 是否停机
        /// </summary>
        public TrueOrFalseEnum IsShutdown { get; set; }

       /// <summary>
        /// 状态 1、待维修 2、已维修3、已确认
        /// </summary>
        public EquRepairOrderStatusEnum Status { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       
    }
}
