using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备故障现象数据实体对象
    ///
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class EquFaultPhenomenonEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :故障现象代码 
        /// 空值 : false  
        /// </summary>
        public string FaultPhenomenonCode { get; set; }
        
        /// <summary>
        /// 描述 :故障现象名称 
        /// 空值 : false  
        /// </summary>
        public string FaultPhenomenonName { get; set; }
        
        /// <summary>
        /// 描述 :设备组ID 
        /// 空值 : false  
        /// </summary>
        public long EquipmentGroupId { get; set; }
        
        /// <summary>
        /// 描述 :使用状态 0-禁用 1-启用 
        /// 空值 : false  
        /// </summary>
        public byte UseStatus { get; set; }
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        }
}