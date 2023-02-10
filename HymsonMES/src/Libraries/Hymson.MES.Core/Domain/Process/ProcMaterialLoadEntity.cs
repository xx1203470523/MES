using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 物料加载表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcMaterialLoadEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        
        /// <summary>
        /// 描述 :所属资源ID 
        /// 空值 : false  
        /// </summary>
        public long ResourceId { get; set; }
        
        /// <summary>
        /// 描述 :所属设备ID 
        /// 空值 : true  
        /// </summary>
        public long? EquipmentId { get; set; }
        
        /// <summary>
        /// 描述 :所属物料表ID 
        /// 空值 : false  
        /// </summary>
        public long MaterialId { get; set; }
        
        /// <summary>
        /// 描述 :所属物料库存表ID(带出 物料条码) 
        /// 空值 : false  
        /// </summary>
        public long MaterialStockId { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        }
}