using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 工作中心数据实体对象
    /// @author wangkeming
    /// @date 2023-02-21
    /// </summary>
    public class InteWorkCenterEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :工作中心代码 
        /// 空值 : false  
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 描述 :工作中心名称 
        /// 空值 : false  
        /// </summary>
        public short Name { get; set; }
        
        /// <summary>
        /// 描述 :类型(工厂/车间/产线) 
        /// 空值 : false  
        /// </summary>
        public short Type { get; set; }
        
        /// <summary>
        /// 描述 :数据来源 
        /// 空值 : false  
        /// </summary>
        public string Source { get; set; }
        
        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// 描述 :是否混线 
        /// 空值 : false  
        /// </summary>
        public byte IsMixLine { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 描述 :站点Id 
        /// 空值 : true  
        /// </summary>
        public long? SiteId { get; set; }
        }
}