using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Quality.QualUnqualifiedGroup.Query
{
    /// <summary>
    /// 不合格代码组分页参数
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedGroupPagedQuery : PagerInfo
    {
        // <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 不合格组
        /// </summary>
        public string? UnqualifiedGroup { get; set; }

        /// <summary>
        /// 不合格组名称
        /// </summary>
        public string? UnqualifiedGroupName { get; set; }
    }
}
