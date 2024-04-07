using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// FQC检验参数组 查询参数
    /// </summary>
    public class QualFqcParameterGroupQuery
    {
        /// <summary>
        /// 排序(默认为 CreatedOn DESC)
        /// </summary>
        public string Sorting { get; set; } = "CreatedOn DESC";

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 状态(0-新建 1-启用 2-保留 3-废除)
        /// 多个产品
        /// </summary>
        public IEnumerable<long>? MaterialIds { get; set; }

        /// <summary>
        /// 状态(1-新建 2-激活 3-保留 4-废除)
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }
    }
}
