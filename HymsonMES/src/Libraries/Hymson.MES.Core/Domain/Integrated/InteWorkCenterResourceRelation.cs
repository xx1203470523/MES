using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 工作中心关联下属资源数据实体对象
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    public class InteWorkCenterResourceRelation : BaseEntity
    {
        /// <summary>
        /// 描述 :工作中心ID 
        /// 空值 : true  
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 描述 :资源ID 
        /// 空值 : true  
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 描述 :站点Id 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
    }
}