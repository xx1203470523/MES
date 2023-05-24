using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated.InteTray.Query
{
    /// <summary>
    /// 托盘信息 分页参数
    /// </summary>
    public class InteTrayPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码（托盘信息）
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称（托盘信息）
        /// </summary>
        public string? Name { get; set; }

    }
}
