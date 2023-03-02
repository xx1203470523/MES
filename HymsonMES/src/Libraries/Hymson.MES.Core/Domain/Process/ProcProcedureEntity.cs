using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 工序BOM表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcProcedureEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :工序BOM代码 
        /// 空值 : false  
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 描述 :工序BOM名称 
        /// 空值 : false  
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public int Status { get; set; }
        
        /// <summary>
        /// 描述 :类型 
        /// 空值 : false  
        /// </summary>
        public int Type { get; set; }
        
        /// <summary>
        /// 描述 :包装等级（字典数据） 
        /// 空值 : true  
        /// </summary>
        public int? PackingLevel { get; set; }
        
        /// <summary>
        /// 描述 :所属资源类型ID 
        /// 空值 : true  
        /// </summary>
        public long? ResourceTypeId { get; set; }
        
        /// <summary>
        /// 描述 :循环次数 
        /// 空值 : true  
        /// </summary>
        public int? Cycle { get; set; }
        
        /// <summary>
        /// 描述 :是否维修返回 
        /// 空值 : true  
        /// </summary>
        public byte IsRepairReturn { get; set; }
        
        /// <summary>
        /// 描述 :版本 
        /// 空值 : true  
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;
    }
}