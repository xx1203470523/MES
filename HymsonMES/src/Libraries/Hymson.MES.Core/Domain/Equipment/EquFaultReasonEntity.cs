using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备故障原因数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class EquFaultReasonEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :故障原因代码 
        /// 空值 : false  
        /// </summary>
        public string FaultReasonCode { get; set; }
        
        /// <summary>
        /// 描述 :故障原因名称 
        /// 空值 : false  
        /// </summary>
        public string FaultReasonName { get; set; }
        
        /// <summary>
        /// 描述 :是否启用 0-禁用 1-启用 
        /// 空值 : false  
        /// </summary>
        public byte UseStatus { get; set; }
        
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