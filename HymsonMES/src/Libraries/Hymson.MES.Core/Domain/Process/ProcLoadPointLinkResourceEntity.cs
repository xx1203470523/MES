using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 上料点关联资源表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcLoadPointLinkResourceEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        
        /// <summary>
        /// 描述 :序号( 程序生成) 
        /// 空值 : true  
        /// </summary>
        public long SerialNo { get; set; }
        
        /// <summary>
        /// 描述 :所属上料点ID 
        /// 空值 : false  
        /// </summary>
        public long LoadPointId { get; set; }
        
        /// <summary>
        /// 描述 :所属资源ID 
        /// 空值 : false  
        /// </summary>
        public long ResourceId { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string? Remark { get; set; }
        }
}