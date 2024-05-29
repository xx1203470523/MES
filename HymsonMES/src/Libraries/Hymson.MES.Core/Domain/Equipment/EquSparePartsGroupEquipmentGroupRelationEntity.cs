using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（备件类型和设备关联关系表）   
    /// equ_spare_parts_group_equipment_group_relation
    /// @author kongaomeng
    /// @date 2023-12-15 11:34:23
    /// </summary>
    public class EquSparePartsGroupEquipmentGroupRelationEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// equ_spare_parts_group 的id
        /// </summary>
        public long? SparePartsGroupId { get; set; }

        /// <summary>
        /// equ_spare_parts_group 的id
        /// </summary>
        public long? SparePartTypeId{ get; set; }

       /// <summary>
        /// equ_equipment_grou  的id
        /// </summary>
        public long? EquipmentGroupId { get; set; }

       
    }
}
