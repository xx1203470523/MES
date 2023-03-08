using Hymson.Infrastructure;
using Hymson.Infrastructure.Constants;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcPrinterPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }
        /// <summary>
        /// 描述 :打印机名称 
        /// 空值 : false  
        /// </summary>
        public string? PrintName { get; set; }

        /// <summary>
        /// 描述 :打印机IP 
        /// 空值 : false  
        /// </summary>
        public string? PrintIp { get; set; }

        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string? Remark { get; set; }
    }
}
