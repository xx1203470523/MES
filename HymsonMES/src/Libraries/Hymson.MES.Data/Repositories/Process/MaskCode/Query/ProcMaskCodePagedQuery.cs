using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process.MaskCode.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcMaskCodePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 掩码编码 
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 掩码名称 
        /// </summary>
        public string? Name { get; set; }
    }
}
