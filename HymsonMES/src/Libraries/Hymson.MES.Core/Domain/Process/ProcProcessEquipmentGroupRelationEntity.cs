using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（设备组关联设备表）   
    /// proc_process_equipment_group_relation
    /// @author Hjy
    /// @date 2023-07-25 10:10:24
    /// </summary>
    public class ProcProcessEquipmentGroupRelationEntity : BaseEntity
    {
        /// <summary>
        /// 备件ID
        /// </summary>
        public long EquipmentGroupId { get; set; }

       /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
