using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（工具类型和设备关联关系表）   
    /// equ_tools_type_equipment_group_relation
    /// @author kongaomeng
    /// @date 2024-6-15 11:34:23
    /// </summary>
    public class EquToolingTypeEquipmentGroupRelationEntity : BaseEntity
    {
        /// <summary>
        /// 类型id equ_tools_type的id
        /// </summary>
        public long? ToolTypeId { get; set; }

        /// <summary>
        /// 设备组id equ_equipment_group 的id
        /// </summary>
        public long? EquipmentGroupId { get; set; }
       
    }
}
