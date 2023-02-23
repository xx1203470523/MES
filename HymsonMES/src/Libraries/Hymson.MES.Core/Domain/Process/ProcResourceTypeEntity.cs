using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 资源类型表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcResourceTypeEntity: BaseEntity
    {   
        /// <summary>
        /// 描述 :资源类型 
        /// 空值 : false  
        /// </summary>
        public string ResType { get; set; }
        
        /// <summary>
        /// 描述 :资源类型名称 
        /// 空值 : false  
        /// </summary>
        public string ResTypeName { get; set; }
        
        /// <summary>
        /// 描述 :描述 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点编id
        /// </summary>
        public long SiteId { get; set; } 
    }
}