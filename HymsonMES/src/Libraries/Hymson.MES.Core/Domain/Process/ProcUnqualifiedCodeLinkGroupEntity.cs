using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 不合格代码关联不合格组表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcUnqualifiedCodeLinkGroupEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        
        /// <summary>
        /// 描述 :所属不合格代码ID 
        /// 空值 : false  
        /// </summary>
        public string UnqualifiedCodeId { get; set; }
        
        /// <summary>
        /// 描述 :所属不合格组ID 
        /// 空值 : false  
        /// </summary>
        public string UnqualifiedGroupId { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        }
}