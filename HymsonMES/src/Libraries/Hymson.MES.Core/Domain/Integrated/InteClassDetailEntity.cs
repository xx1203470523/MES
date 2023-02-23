using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 班制维护详情数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteClassDetailEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :班次表id 
        /// 空值 : false  
        /// </summary>
        public long ClassId { get; set; }
        
        /// <summary>
        /// 描述 :班次（字典名称：manu_detail_class_type） 
        /// 空值 : false  
        /// </summary>
        public string DetailClassType { get; set; }
        
        /// <summary>
        /// 描述 :项目内容 
        /// 空值 : false  
        /// </summary>
        public string ProjectContent { get; set; }
        
        /// <summary>
        /// 描述 :开始时间 
        /// 空值 : false  
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// 描述 :结束时间 
        /// 空值 : false  
        /// </summary>
        public DateTime EndTime { get; set; }
        
        /// <summary>
        /// 描述 :描述 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        }
}