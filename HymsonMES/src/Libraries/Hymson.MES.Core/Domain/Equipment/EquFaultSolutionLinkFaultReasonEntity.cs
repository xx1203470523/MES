using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备故障解决措施关联设备故障原因数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class EquFaultSolutionLinkFaultReasonEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :设备故障原因id 
        /// 空值 : false  
        /// </summary>
        public long FaultReasonId { get; set; }
        
        /// <summary>
        /// 描述 :设备故障解决措施id 
        /// 空值 : false  
        /// </summary>
        public long FaultSolutionId { get; set; }
        
        /// <summary>
        /// 描述 :设备故障先 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        }
}