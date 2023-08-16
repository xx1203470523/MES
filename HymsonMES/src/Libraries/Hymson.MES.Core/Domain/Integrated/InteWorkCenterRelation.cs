using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 工作中心关联下属中心数据实体对象
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    public class InteWorkCenterRelation: BaseEntity
    {
        /// <summary>
        /// 描述 :所属工作中心ID（父工作中心） 
        /// 空值 : false  
        /// </summary>
        public long WorkCenterId { get; set; }
        
        /// <summary>
        /// 描述 :工作中心ID 
        /// 空值 : true  
        /// </summary>
        public long SubWorkCenterId { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 描述 :站点Id 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        }
}