using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 工序BOM配置打印表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcProcedurePrintRelationEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :份数 
        /// 空值 : true  
        /// </summary>
        public int? Copy { get; set; }
        
        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";
        
        /// <summary>
        /// 描述 :所属工序ID 
        /// 空值 : false  
        /// </summary>
        public long ProcedureId { get; set; }
        
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
        /// 描述 :所属模板ID 
        /// 空值 : true  
        /// </summary>
        public long TemplateId { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;
    }
}