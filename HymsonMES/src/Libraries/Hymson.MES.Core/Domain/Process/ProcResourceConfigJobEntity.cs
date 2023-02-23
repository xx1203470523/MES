using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 资源作业配置表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcResourceConfigJobEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        
        /// <summary>
        /// 描述 :所属资源ID 
        /// 空值 : false  
        /// </summary>
        public long ResourceId { get; set; }
        
        /// <summary>
        /// 描述 :关联点(字典key值) 
        /// 空值 : true  
        /// </summary>
        public string LinkPoint { get; set; }
        
        /// <summary>
        /// 描述 :作业ID 
        /// 空值 : false  
        /// </summary>
        public long JobId { get; set; }
        
        /// <summary>
        /// 描述 :是否启用 
        /// 空值 : true  
        /// </summary>
        public bool IsUse { get; set; }
        
        /// <summary>
        /// 描述 :参数 
        /// 空值 : true  
        /// </summary>
        public string Parameter { get; set; }
        
        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        }
}