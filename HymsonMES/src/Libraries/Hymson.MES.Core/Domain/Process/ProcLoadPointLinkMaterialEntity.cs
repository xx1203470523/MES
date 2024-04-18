using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 上料点关联物料表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcLoadPointLinkMaterialEntity: BaseEntity
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
        /// 描述 :所属上料点ID 
        /// 空值 : false  
        /// </summary>
        public long LoadPointId { get; set; }
        
        /// <summary>
        /// 描述 :所属物料ID 
        /// 空值 : false  
        /// </summary>
        public long MaterialId { get; set; }
        
        /// <summary>
        /// 描述 :版本 
        /// 空值 : true  
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// 描述 :参考点 
        /// 空值 : true  
        /// </summary>
        public string ReferencePoint { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string? Remark { get; set; }
        }
}