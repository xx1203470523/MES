using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.System
{
    /// <summary>
    /// 站点配置表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class SysSiteConfigEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :站点(代码) 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        
        /// <summary>
        /// 描述 :站点名称 
        /// 空值 : false  
        /// </summary>
        public string SiteName { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        }
}