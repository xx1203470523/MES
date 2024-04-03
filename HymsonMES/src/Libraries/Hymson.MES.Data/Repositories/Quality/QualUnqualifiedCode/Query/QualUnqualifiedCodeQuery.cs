using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 不合格代码查询参数
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedCodeQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 不合格代码组id
        /// </summary>
        public long? UnqualifiedGroupId { get; set; }

        /// <summary>
        /// 不合格代码组id列表
        /// </summary>
        public IEnumerable<long>? UnqualifiedGroupIds { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 状态列表
        /// </summary>
        public SysDataStatusEnum[]? StatusArr { get; set; }
    }
}
