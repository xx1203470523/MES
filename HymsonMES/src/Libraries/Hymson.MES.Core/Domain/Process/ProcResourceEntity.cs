using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 资源维护表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcResourceEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        
        /// <summary>
        /// 描述 :资源代码 
        /// 空值 : false  
        /// </summary>
        public string ResCode { get; set; }
        
        /// <summary>
        /// 描述 :资源名称 
        /// 空值 : false  
        /// </summary>
        public string ResName { get; set; }
        
        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public int Status { get; set; }
        
        /// <summary>
        /// 描述 :所属资源类型ID 
        /// 空值 : false  
        /// </summary>
        public long ResTypeId { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";
    }
}