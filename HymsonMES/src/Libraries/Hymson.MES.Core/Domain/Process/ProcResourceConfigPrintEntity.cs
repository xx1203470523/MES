using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 资源配置打印机表数据实体对象
    ///
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcResourceConfigPrintEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        
        /// <summary>
        /// 描述 :所属资源ID 
        /// 空值 : false  
        /// </summary>
        public long ResourceId { get; set; }
        
        /// <summary>
        /// 描述 :打印机ID 
        /// 空值 : false  
        /// </summary>
        public long PrintId { get; set; }
        }
}