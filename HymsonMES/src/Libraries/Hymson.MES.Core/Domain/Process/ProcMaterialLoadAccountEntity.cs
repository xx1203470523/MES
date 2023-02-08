using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 物料加载台账表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcMaterialLoadAccountEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        
        /// <summary>
        /// 描述 :所属物料装载ID 
        /// 空值 : false  
        /// </summary>
        public long MaterialLoadId { get; set; }
        
        /// <summary>
        /// 描述 :流转类型(物料加载,...) 
        /// 空值 : false  
        /// </summary>
        public string CirculationType { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        }
}