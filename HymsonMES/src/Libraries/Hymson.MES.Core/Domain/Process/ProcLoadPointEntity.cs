using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 上料点表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcLoadPointEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        
        /// <summary>
        /// 描述 :上料点 
        /// 空值 : false  
        /// </summary>
        public string LoadPoint { get; set; }
        
        /// <summary>
        /// 描述 :上料点名称 
        /// 空值 : false  
        /// </summary>
        public string LoadPointName { get; set; }
        
        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public int Status { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string? Remark { get; set; }
        }
}