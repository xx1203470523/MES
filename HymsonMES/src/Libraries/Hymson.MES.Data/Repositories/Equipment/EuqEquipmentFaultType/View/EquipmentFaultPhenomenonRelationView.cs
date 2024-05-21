namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备故障类型关联故障现象视图
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class EquipmentFaultPhenomenonRelationView
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障类型ID;equ_fault_type的Id
        /// </summary>
        public long FaultTypeId { get; set; }

        // <summary>
        /// 故障现象ID;equ_fault_phenomenon的Id
        /// </summary>
        public long FaultPhenomenonId { get; set; }

        /// <summary>
        /// 故障现象代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 故障现象名称
        /// </summary>
        public string Name { get; set; }

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
