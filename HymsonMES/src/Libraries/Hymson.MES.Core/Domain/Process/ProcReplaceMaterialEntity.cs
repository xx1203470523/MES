using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 物料替代组件表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcReplaceMaterialEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        
        /// <summary>
        /// 描述 :物料ID 
        /// 空值 : false  
        /// </summary>
        public long MaterialId { get; set; }
        
        /// <summary>
        /// 描述 :替代组件ID 
        /// 空值 : false  
        /// </summary>
        public long ReplaceMaterialId { get; set; }
        
        /// <summary>
        /// 描述 :是否启用 
        /// 空值 : true  
        /// </summary>
        public byte IsUse { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        }
}