using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// BOM表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcBomEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        
        /// <summary>
        /// 描述 :BOM 
        /// 空值 : false  
        /// </summary>
        public string BomCode { get; set; }
        
        /// <summary>
        /// 描述 :BOM名称 
        /// 空值 : false  
        /// </summary>
        public string BomName { get; set; }
        
        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public SysDataStatusEnum Status { get; set; }
        
        /// <summary>
        /// 描述 :版本 
        /// 空值 : false  
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// 描述 :是否当前版本 
        /// 空值 : true  
        /// </summary>
        public bool IsCurrentVersion { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string? Remark { get; set; }
        }
}