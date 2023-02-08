using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 不合格代码作业配置表数据实体对象
    ///
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class QualUnqualifiedCodeConfigJobEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        
        /// <summary>
        /// 描述 :所属不合格代码ID 
        /// 空值 : false  
        /// </summary>
        public long UnqualifiedCodeId { get; set; }
        
        /// <summary>
        /// 描述 :序号 
        /// 空值 : true  
        /// </summary>
        public string OrderNumber { get; set; }
        
        /// <summary>
        /// 描述 :作业ID 
        /// 空值 : false  
        /// </summary>
        public long JobId { get; set; }
        
        /// <summary>
        /// 描述 :是否启用 
        /// 空值 : true  
        /// </summary>
        public byte IsUse { get; set; }
        
        /// <summary>
        /// 描述 :参数 
        /// 空值 : true  
        /// </summary>
        public string Parameter { get; set; }
        }
}