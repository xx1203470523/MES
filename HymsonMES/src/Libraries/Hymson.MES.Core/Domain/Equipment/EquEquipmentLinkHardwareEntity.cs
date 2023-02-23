using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备关联硬件设备数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class EquEquipmentLinkHardwareEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :设备id（equ_equipment表id） 
        /// 空值 : false  
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 描述 :硬件设备唯一标识 
        /// 空值 : false  
        /// </summary>
        public string HardwareCode { get; set; }

        /// <summary>
        /// 描述 :硬件类型（字段名称：equ_hardware_type） 
        /// 空值 : false  
        /// </summary>
        public string HardwareType { get; set; }

        /// <summary>
        /// 描述 :设备故障先 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
    }
}