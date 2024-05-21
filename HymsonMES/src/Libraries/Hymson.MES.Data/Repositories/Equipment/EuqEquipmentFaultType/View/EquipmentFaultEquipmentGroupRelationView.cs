namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备故障类型关联设备组视图
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class EquipmentFaultEquipmentGroupRelationView
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障类型ID;equ_fault_type的Id
        /// </summary>
        public long FaultTypeId { get; set; }

        /// <summary>
        /// 设备组ID;equ_equipment_group的Id
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
