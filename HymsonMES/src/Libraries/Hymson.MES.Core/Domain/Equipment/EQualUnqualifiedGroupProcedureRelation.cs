using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备故故障类型关联设备组
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class EQualUnqualifiedGroupProcedureRelation : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :故障类型ID;equ_fault_type的Id
        /// 空值 : false  
        /// </summary>
        public long FaultTypeId { get; set; }

        /// <summary>
        /// 描述 :设备组ID;equ_equipment_group的Id 
        /// 空值 : false  
        /// </summary>
        public long EquipmentGroupId { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";
    }
}