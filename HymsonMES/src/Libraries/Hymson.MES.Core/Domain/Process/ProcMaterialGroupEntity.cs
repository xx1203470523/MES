using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 物料组维护表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcMaterialGroupEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        
        /// <summary>
        /// 描述 :物料组编号 
        /// 空值 : false  
        /// </summary>
        public string GroupCode { get; set; }
        
        /// <summary>
        /// 描述 :物料组名称 
        /// 空值 : false  
        /// </summary>
        public string GroupName { get; set; }
        
        /// <summary>
        /// 描述 :物料组版本 
        /// 空值 : true  
        /// </summary>
        public string GroupVersion { get; set; }
        
        /// <summary>
        /// 描述 :物料组描述 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        }
}