using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 设备故障解决措施表数据实体对象
    ///
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class EquFaultSolutionEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :故障解决措施代码 
        /// 空值 : false  
        /// </summary>
        public string FaultSolutionCode { get; set; }
        
        /// <summary>
        /// 描述 :故障解决措施代码 
        /// 空值 : false  
        /// </summary>
        public string FaultSolutionName { get; set; }
        
        /// <summary>
        /// 描述 :是否启用 0-禁用 1-启用 
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