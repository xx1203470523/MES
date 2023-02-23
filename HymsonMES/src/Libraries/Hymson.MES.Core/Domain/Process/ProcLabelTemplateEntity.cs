using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 标签模板表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcLabelTemplateEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        
        /// <summary>
        /// 描述 :标签模板 
        /// 空值 : false  
        /// </summary>
        public string TemplateCode { get; set; }
        
        /// <summary>
        /// 描述 :模板名称 
        /// 空值 : false  
        /// </summary>
        public string TemplateName { get; set; }
        
        /// <summary>
        /// 描述 :类型 
        /// 空值 : false  
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// 描述 :长度 
        /// 空值 : false  
        /// </summary>
        public decimal Length { get; set; }
        
        /// <summary>
        /// 描述 :宽度 
        /// 空值 : false  
        /// </summary>
        public decimal Width { get; set; }
        
        /// <summary>
        /// 描述 :单位（字典配置） 
        /// 空值 : false  
        /// </summary>
        public string Unit { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        }
}