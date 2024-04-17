using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 工作中心关联下属中心或资源表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteWorkCenterLinkEntity: BaseEntity
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
        /// 描述 :所属工作中心ID（父工作中心） 
        /// 空值 : false  
        /// </summary>
        public long WorkCenterId { get; set; }
        
        /// <summary>
        /// 描述 :工作中心ID 
        /// 空值 : true  
        /// </summary>
        public long? SubWorkCenterId { get; set; }
        
        /// <summary>
        /// 描述 :资源ID 
        /// 空值 : true  
        /// </summary>
        public long? SubResourceId { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";
        }
}