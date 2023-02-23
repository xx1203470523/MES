using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备关联接口数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class EquEquipmentLinkApiEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :设备id（equ_equipment表id） 
        /// 空值 : false  
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 描述 :接口地址 
        /// 空值 : false  
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 描述 :接口类型（字段名称：equ_api_type） 
        /// 空值 : false  
        /// </summary>
        public string ApiType { get; set; }

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