using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// FQC不合格处理结果 查询参数
    /// </summary>
    public class QualFqcOrderUnqualifiedHandleQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 主键集合
        /// </summary>
        public IEnumerable<long>? FQCOrderIds { get; set; }

        /// <summary>
        /// 不合格处理方式
        /// </summary>
        public FQCHandMethodEnum? HandMethod { get; set; }
    }
}
