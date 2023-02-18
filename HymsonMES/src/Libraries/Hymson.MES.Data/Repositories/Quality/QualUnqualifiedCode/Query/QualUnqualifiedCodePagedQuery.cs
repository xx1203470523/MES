using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query
{
    /// <summary>
    /// 不合格代码分页参数
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedCodePagedQuery : PagerInfo
    {
        // <summary>
        /// 工厂
        /// </summary>
        public string SiteCode { get; set; }
        // <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public short? Type { get; set; }
    }
}
