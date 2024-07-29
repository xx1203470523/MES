using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment.EquMaintenance
{
    /// <summary>
    /// 设备点检模板与设备组关系，数据实体对象   
    /// equ_maintenance_template_equipment_group_relation
    /// @author pengxin
    /// @date 2024-05-23 03:22:22
    /// </summary>
    public class EquMaintenanceTemplateEquipmentGroupRelationEntity : BaseEntity 
    {
        /// <summary>
        /// 点检模板ID;equ_maintenance_template的Id
        /// </summary>
        public long MaintenanceTemplateId { get; set; } 

        /// <summary>
        /// 设备组ID;equ_equipment_group的Id
        /// </summary>
        public long EquipmentGroupId { get; set; }


    }
}
