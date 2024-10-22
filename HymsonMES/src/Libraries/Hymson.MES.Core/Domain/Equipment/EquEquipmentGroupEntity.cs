using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备组数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class EquEquipmentGroupEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :设备组编码 
        /// 空值 : false  
        /// </summary>
        public string EquipmentGroupCode { get; set; }

        /// <summary>
        /// 描述 :设备组名称 
        /// 空值 : false  
        /// </summary>
        public string EquipmentGroupName { get; set; }

        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 站点ID 
        /// </summary>
        public long? SiteId { get; set; }
    }
}