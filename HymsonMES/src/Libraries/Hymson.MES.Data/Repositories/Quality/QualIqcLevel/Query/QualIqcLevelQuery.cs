using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// IQC检验水平 查询参数
    /// </summary>
    public class QualIqcLevelQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设置类型 1、通用 2、物料
        /// </summary>
        public QCMaterialTypeEnum Type { get; set; }

    }
}
