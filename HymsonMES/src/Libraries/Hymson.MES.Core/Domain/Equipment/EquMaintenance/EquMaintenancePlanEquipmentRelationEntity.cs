using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment.EquMaintenance
{
    /// <summary>
    /// 设备点检计划与设备关系，数据实体对象   
    /// equ_maintenance_plan_equipment_relation
    /// @author pengxin
    /// @date 2024-05-23 03:08:03
    /// </summary>
    public class EquMaintenancePlanEquipmentRelationEntity : BaseEntity 
    {
        /// <summary>
        /// 点检计划ID;equ_maintenance_plan表的Id
        /// </summary>
        public long MaintenancePlanId { get; set; }

        /// <summary>
        /// 点检模板ID;equ_maintenance_template表的Id
        /// </summary>
        public long MaintenanceTemplateId { get; set; }

        /// <summary>
        /// 设备ID;equ_equipment的Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 点检执行人;用户中心UserId集合
        /// </summary>
        public string? ExecutorIds { get; set; }

        /// <summary>
        /// 点检负责人;用户中心UserId集合
        /// </summary>
        public string? LeaderIds { get; set; }
    }
}
