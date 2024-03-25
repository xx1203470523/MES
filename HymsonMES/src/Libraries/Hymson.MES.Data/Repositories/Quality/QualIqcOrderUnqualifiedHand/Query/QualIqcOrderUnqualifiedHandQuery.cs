using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// Iqc不合格处理 查询参数
    /// </summary>
    public class QualIqcOrderUnqualifiedHandQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 主键集合
        /// </summary>
        public IEnumerable<long>? IQCOrderIds { get; set; }

        /// <summary>
        /// 不合格处理方式
        /// </summary>
        public IQCHandMethodEnum? HandMethod { get; set; }

    }
}
