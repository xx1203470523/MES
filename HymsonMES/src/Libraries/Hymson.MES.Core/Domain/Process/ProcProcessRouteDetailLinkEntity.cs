using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 工艺路线工序节点关系明细表(前节点多条就存多条)数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcProcessRouteDetailLinkEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        
        /// <summary>
        /// 描述 :序号 
        /// 空值 : true  
        /// </summary>
        public long SerialNo { get; set; }
        
        /// <summary>
        /// 描述 :所属工艺路线ID 
        /// 空值 : false  
        /// </summary>
        public long ProcessRouteId { get; set; }
        
        /// <summary>
        /// 描述 :前一工艺路线工序明细ID 
        /// 空值 : true  
        /// </summary>
        public long? PreProcessRouteDetailId { get; set; }
        
        /// <summary>
        /// 描述 :当前工艺路线工序明细ID 
        /// 空值 : false  
        /// </summary>
        public long ProcessRouteDetailId { get; set; }
        
        /// <summary>
        /// 描述 :扩展字段1(暂存坐标) 
        /// 空值 : true  
        /// </summary>
        public string Extra1 { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";
        }
}