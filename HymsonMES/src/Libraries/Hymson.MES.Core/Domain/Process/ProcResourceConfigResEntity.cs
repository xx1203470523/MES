using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 资源配置表数据实体对象
    ///
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcResourceConfigResEntity: BaseEntity
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
        /// 描述 :设置类型(字典配置) 
        /// 空值 : true  
        /// </summary>
        public string SetType { get; set; }
        
        /// <summary>
        /// 描述 :设置值 
        /// 空值 : false  
        /// </summary>
        public string Value { get; set; }
        }
}